using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;


class GPTUtil
{
    // public void requestGPT(string prompt)
    // {
    //     Debug.Log("requestGPT "+prompt);
    //     // 设置请求的URL和参数
    //     string url = "https://api.openai.com/v1/engines/davinci-codex/completions";
    //     int maxTokens = 50;
    //     bool returnSequences = true;

    //     // 设置请求的headers
    //     var headers = new Dictionary<string, string>
    //     {
    //         { "Content-Type", "application/json" },
    //         { "Authorization", "Bearer your_api_key_here" } // 请替换成你自己的API Key
    //     };

    //     // 设置请求的payload
    //     var data = new
    //     {
    //         prompt = prompt,
    //         max_tokens = maxTokens,
    //         return_sequences = returnSequences
    //     };

    //     // 发送POST请求
    //     using var client = new HttpClient();
    //     var json = JsonConvert.SerializeObject(data);
    //     var content = new StringContent(json, Encoding.UTF8, "application/json");
    //     foreach (var header in headers)
    //     {
    //         client.DefaultRequestHeaders.Add(header.Key, header.Value);
    //     }
    //     using var response = await client.PostAsync(url, content);
    //     var responseContent = await response.Content.ReadAsStringAsync();

    //     // 处理响应结果
    //     if (response.IsSuccessStatusCode)
    //     {
    //         Console.WriteLine(responseContent);
    //     }
    //     else
    //     {
    //         Console.WriteLine($"请求失败：{response.StatusCode} - {responseContent}");
    //     }
    // }


    public void prepare(string rule) {

    }

    public string chat(string prompt) {
        Debug.Log("chat: "+prompt);
            string endpoint = "https://api.openai.com/v1/engines/davinci-codex/completions";
            string apiKey = "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";
            int maxTokens = 50;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "POST";
            request.Headers.Add("Authorization", "Bearer " + apiKey);

            // Set up the request body
            string requestBody = "{\"prompt\": \"" + prompt + "\", \"max_tokens\": " + maxTokens + "}";
            byte[] requestBodyBytes = Encoding.UTF8.GetBytes(requestBody);
            request.ContentLength = requestBodyBytes.Length;
            request.ContentType = "application/json";

            // Send the request
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(requestBodyBytes, 0, requestBodyBytes.Length);
            }
            Debug.Log("molly Send the request: ");
            // Get the response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string responseText = reader.ReadToEnd();
                        JObject responseJson = JObject.Parse(responseText);
                        var choices = responseJson["choices"][0];
                        string message = (string)choices["text"];
                        Debug.Log("molly choices: "+choices);
                        return message;

                    }
                }
            }
        }

    public async Task chatFun(string prompt) {
        Debug.Log("chat: "+prompt);
        HttpClient client = new HttpClient();
         try 
            {
                string baseUrl = "https://api.chatgpt.com/v1"; // API基础URL
                string endpoint = "/chat"; // API端点
                string token = "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"; // 您的API访问令牌
                string input = "Hello, how are you?"; // 聊天输入

                string url = $"{baseUrl}{endpoint}?input={input}&token={token}"; // 构建请求URL

                HttpResponseMessage response = await client.GetAsync(url); // 发送GET请求

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync(); // 解析响应数据
                    Console.WriteLine("molly "+responseBody);
                }
                else
                {
                    Console.WriteLine($"请求失败，状态码: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"请求出错: {e.Message}");
            }

    }






}