// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "非真实感渲染/卡通渲染"
{
    Properties
    {
        _Color ("主色调", Color) = (1, 1, 1, 1)
        _MainTex ("贴图", 2D) = "white" {}

        _Specular ("高光颜色", Color) = (1, 1, 1, 1)
        _SpecularScale ("高光程度", Range(0, 0.1)) = 0.01
    }
    
    SubShader
    {
        
        Pass{
            Tags{"LightMode" = "ForwardBase"}
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // 加上之后会出现无法解决的透视效果
            // #pragma multi_compile_fwdbase
            
            // 得到正确的光照
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Specular;
            fixed _SpecularScale;

            struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
                float4 tangent : TANGENT;
			};

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                SHADOW_COORDS(3)
            };

            v2f vert(a2v v)
            {
                v2f o;
                // 转换顶点坐标，打上纹理
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                TRANSFER_SHADOW(o);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target{
                // 获得单位向量
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                fixed3 worldHalfDir = normalize(worldLightDir + worldViewDir);
                
                // 反照率与环境光
                fixed3 albedo = tex2D(_MainTex, i.uv).rgb * _Color.rgb;
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
                
                // 光照衰减，获得更好的阴影过渡（漫反射）
                UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);
                fixed diff = dot(worldNormal, worldLightDir);
                diff = (diff * 0.5 + 0.5) * atten;
                // 漫反射受渐变纹理影响
                fixed3 diffuse = _LightColor0.rgb * albedo;

                // half-lambert高光
                // 计算高光方向
                fixed spec = dot(worldNormal, worldHalfDir);
                // 高光抗锯齿
                fixed w = fwidth(spec) * 2.0;
                // 滤掉不需要的半兰伯特高光spec。step的作用是当scale为0时高光完全消失
                fixed3 specular = _Specular.rgb * lerp(0, 1, smoothstep(-w, w, spec + _SpecularScale - 1))
                    * step(0.0001, _SpecularScale);

                return fixed4(ambient + diffuse + specular, 1.0);
            }
            ENDCG
        }
    }
}
