using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GPTClient
{
    class Program
    {
        public static async Task chat(string input)
        {
            Debug.Log("molly chat "+input);

            string apiUrl = "https://api.openai.com/v1/chat/completions";
            string apiKey = "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";

            Request request = new Request();
            request.Messages = new RequestMessage[]
            {
                new RequestMessage()
                {
                     Role = "system",
                     Content = "You are a helpful assistant."
                },
                new RequestMessage()
                {
                     Role = "user",
                     Content = "Who won the world series in 2020?"
                }
            };

            string requestData = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

            using (HttpClient httpClient = new HttpClient())
            {
                Debug.Log("molly before response");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(apiUrl, content);
                Debug.Log("molly after response");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                    Response response = JsonConvert.DeserializeObject<Response>(responseString);
                    Debug.Log("molly response: "+response.Choices[0].Message.Content);
                }
                else
                {
                    Debug.Log($"molly Error: {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}");
                }
            }
        }

        public static async Task chat1(string input)
        {
            Debug.Log("molly chat1 "+input);

            // Replace YOUR_API_KEY with your actual API key
            string apiKey = "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";
            string apiUrl = "https://api.gpt-3.5-turbo.com";

            // Create an HTTP client and set the API URL and method
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(apiUrl);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // Add any request parameters to the request body
            string requestBody = "{ \"prompt\": \"Hello, world!\" }";

            // Send the HTTP request and get the response
            HttpResponseMessage response = await httpClient.PostAsync("/v1/completions", new StringContent(requestBody));

            // Read the response content
            string responseContent = await response.Content.ReadAsStringAsync();

            // Output the response content to the console
            Console.WriteLine(responseContent);
            Debug.Log("molly chat1 succ");
        }

        // public static async Task chat2() {
        //     Debug.Log("molly chat2 ");
        //     string apiKey = "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";
        //     var client = new HttpClient();
        //     client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        //     {

        //     var content = new StringContent("{\"model\": \"text-davinci-003\", \"prompt\": \"Say this is a test\", \"temperature\": 0, \"max_tokens\":7}", Encoding.UTF8, "application/json");
        //     // var response = client.PostAsync("https://api.openai.com/v1/completions", content).Result;
        //     // var responseString = response.Content.ReadAsStringAsync().Result;

        //     HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content).Result;
        //     string responseString = await response.Content.ReadAsStringAsync().Result;


        //     // dynamic responseJson = JsonConvert.DeserializeObject(responseString);
        //     // string completion = responseJson.choices[0].text;
        //     Debug.Log("molly chat2 "+responseString);
        //     Console.WriteLine("Completion: " + responseString);
        //     }
        // }
    }


    public class Request
    {
        [JsonProperty("model")]
        public string Model { get; set; } = "gpt-3.5-turbo";
        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; } = 4000;
        [JsonProperty("messages")]
        public RequestMessage[] Messages { get; set; }
    }

    public class RequestMessage
    {
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class Response
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("created")]
        public int Created { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("usage")]
        public ResponseUsage Usage { get; set; }
        [JsonProperty("choices")]
        public ResponseChoice[] Choices { get; set; }
    }

    public class ResponseUsage
    {
        [JsonProperty("prompt_tokens")]
        public int PromptTokens { get; set; }
        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; set; }
        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }
    }

    public class ResponseChoice
    {
        [JsonProperty("message")]
        public ResponseMessage Message { get; set; }
        [JsonProperty("finish_reason")]
        public string FinishReason { get; set; }
        [JsonProperty("index")]
        public int Index { get; set; }
    }

    public class ResponseMessage
    {
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
    }

}
