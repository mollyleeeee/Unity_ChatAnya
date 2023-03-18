using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SocketClient
{
    public static async Task<string> request(string prompt)
    {   
        Debug.Log ("request: "+prompt);
        try
        {
            // 设置服务器地址和端口号
            IPAddress serverAddress = IPAddress.Parse("127.0.0.1");
            int serverPort = 52277;

            // 创建socket对象
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 连接服务器
            await clientSocket.ConnectAsync(serverAddress, serverPort);

            // 发送请求数据
            byte[] requestBytes = Encoding.UTF8.GetBytes(prompt);
            await clientSocket.SendAsync(new ArraySegment<byte>(requestBytes), SocketFlags.None);

            // 接收响应数据
            byte[] responseBuffer = new byte[1024];
            int bytesRead = await clientSocket.ReceiveAsync(new ArraySegment<byte>(responseBuffer), SocketFlags.None);
            string responseString = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);

            // // 解析响应参数
            // string[] responseParams = responseString.Split('&');
            // var responseDict = new Dictionary<string, string>();
            // foreach (string param in responseParams)
            // {
            //     string[] keyValue = param.Split('=');
            //     responseDict[keyValue[0]] = keyValue[1];
            // }

            // // 输出解析结果
            // foreach (var pair in responseDict)
            // {
            //     Debug.Log ($"{pair.Key}: {pair.Value}");
            // }

            // 关闭socket连接
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            Debug.Log ("response: "+responseString+" end");
            return responseString;

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return "";
        }
    }

}
