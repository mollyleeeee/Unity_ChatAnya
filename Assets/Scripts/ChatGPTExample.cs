using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace ChatGPTExampleName
{

    class ChatGPTExample
    {
        private static string API_KEY="";

        public static async Task<string> chat(string prompt)
        {
            Debug.Log("chat satrt: "+prompt);
            if(API_KEY == "") {
                parseConfig();
            }
            string apiUrl = "https://api.openai.com/v1/engines/text-davinci-003/completions";

            string requestBody = JsonConvert.SerializeObject(new
            {
                prompt = prompt,
                max_tokens = 50,
                temperature = 0.7
            });

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "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");

                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Debug.Log("chat succ "+responseContent);
                    return responseContent;
                }
                else
                {
                    Debug.Log("chat error "+ response.StatusCode);
                    return "SERVER ERROR";
                }
            }

            Console.ReadLine();
        }

    public static async void parseConfig() {
        string filePath = "config.txt";
        string apiKey = "";

        try {
            // 读取文件内容
            using (StreamReader sr = new StreamReader(filePath)) {
                string line = sr.ReadLine();

                // 解析api_key
                if (line.StartsWith("api_key=")) {
                    apiKey = line.Substring(8);
                }
            }
        } catch (Exception e) {
            Debug.Log("读取文件失败: " + e.Message);
            return;
        }
        API_KEY = apiKey;
        Debug.Log("API密钥是: " + apiKey);

    }
    }
}