using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace YuoTools.Extend.AI
{
    public class DoubaoApi
    {
        public const string URL = "https://ark.cn-beijing.volces.com/api/v3/chat/completions";

        static async Task GetAwaiter(AsyncOperation asyncOperation)
        {
            var task = new TaskCompletionSource<bool>();
            asyncOperation.completed += _ => { task.SetResult(true); };
            await task.Task;
        }

        static async IAsyncEnumerable<string> GetResponseStream(string body, List<(string, string)> headers = null)
        {
            if (AIHelper.ApiKey.IsNullOrSpace())
            {
                Debug.LogError("请输入ApiKey");
                yield break;
            }

            UnityWebRequest webRequest = new UnityWebRequest(URL, "POST");

            headers ??= new List<(string, string)>
            {
                ("Content-Type", "application/json"),
                ("Authorization", $"Bearer {AIHelper.ApiKey}")
            };
            foreach (var header in headers)
            {
                webRequest.SetRequestHeader(header.Item1, header.Item2);
            }

            byte[] jsonToSend = new UTF8Encoding().GetBytes(body);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.disposeDownloadHandlerOnDispose = true;
            webRequest.disposeUploadHandlerOnDispose = true;

            $"Sending request to: {webRequest.url} with body: {body} and headers: ".Log();

            var asyncOption = webRequest.SendWebRequest();
            var messageDelay = 100;
            int safeCount = 1000;
            string result = "";
            while (!asyncOption.isDone)
            {
                await Task.Delay(messageDelay);

                string OnJson(ChatCompletionStreamResponse json)
                {
                    return json.choices[0].ToString();
                }

                result = OperateMessage(webRequest.downloadHandler.text, OnJson);
                result = RemoveStartSpace(result);
                yield return result;
                if (safeCount-- < 0)
                {
                    Debug.LogError("请求超时");
                    break;
                }
            }

            webRequest.Dispose();
        }

        public static async IAsyncEnumerable<string> GenerateStream(string prompt)
        {
            var model = AIHelper.AIModel;
            if (string.IsNullOrEmpty(model))
            {
                Debug.LogError("模型不能为空");
                yield break;
            }

            var requestBody = new chatRequest(model, prompt)
            {
                stream = true
            };
            string body = JsonConvert.SerializeObject(requestBody);
            await foreach (var line in GetResponseStream(body))
            {
                if (line != null) yield return line;
            }
        }


        public static async Task<string> GenerateText(string prompt)
        {
            var model = AIHelper.AIModel;
            if (string.IsNullOrEmpty(model))
            {
                Debug.LogError("模型不能为空");
                return "模型不能为空";
            }

            var requestBody = new chatRequest(model, prompt);

            string body = JsonConvert.SerializeObject(requestBody);
            var responseJson = await GetResponse(body);

            if (string.IsNullOrEmpty(responseJson)) return "发生错误";
            var parsedJson = JsonConvert.DeserializeObject<ChatCompletionResponse>(responseJson);
            return parsedJson.choices[0].message.content;
        }

        static async Task<string> GetResponse(string body, List<(string, string)> headers = null)
        {
            if (AIHelper.ApiKey.IsNullOrSpace())
            {
                Debug.LogError("请输入ApiKey");
                return null;
            }

            UnityWebRequest webRequest = new UnityWebRequest(URL, "POST");

            headers ??= new List<(string, string)>
            {
                ("Content-Type", "application/json"),
                ("Authorization", $"Bearer {AIHelper.ApiKey}")
            };
            foreach (var header in headers)
            {
                webRequest.SetRequestHeader(header.Item1, header.Item2);
            }

            byte[] jsonToSend = new UTF8Encoding().GetBytes(body);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.disposeDownloadHandlerOnDispose = true;
            webRequest.disposeUploadHandlerOnDispose = true;

            await GetAwaiter(webRequest.SendWebRequest());

            string result = "";
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    result = webRequest.downloadHandler.text;
                    break;
            }

            webRequest.Dispose();
            return result;
        }

        public static string RemoveStartSpace(string str)
        {
            int i = 0;
            for (; i < str.Length; i++)
            {
                if (str[i] != ' ' && str[i] != '\n')
                    break;
            }

            return str.Substring(i);
        }

        public static string OperateMessage(string str, Func<ChatCompletionStreamResponse, string> onJson)
        {
            string split = "data:";
            var lines = str.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder result = new StringBuilder();
            foreach (var line in lines)
            {
                if (line.Trim() == "[DONE]")
                {
                    break;
                }

                try
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var trimmedLine = line.Trim();
                        if (trimmedLine.StartsWith("{") && trimmedLine.EndsWith("}"))
                        {
                            var json = JsonConvert.DeserializeObject<ChatCompletionStreamResponse>(trimmedLine);
                            if (json is { choices: { Count: > 0 } })
                            {
                                //处理新的返回结构
                                result.Append(onJson?.Invoke(json));
                            }
                        }
                    }
                }
                catch (JsonReaderException e)
                {
                    Debug.LogError($"JSON解析错误: {e.Message} - Line: {line}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"处理错误: {e.Message}");
                }
            }

            return result.ToString();
        }

        [Serializable]
        public class ChatCompletionResponse
        {
            public string id { get; set; }
            public string @object { get; set; }
            public long created { get; set; }
            public string model { get; set; }
            public List<Choice> choices { get; set; }
            public Usage usage { get; set; }
        }

        public class ChatCompletionStreamResponse
        {
            public string id { get; set; }
            public string @object { get; set; }
            public long created { get; set; }
            public string model { get; set; }
            public List<StreamChoice> choices { get; set; }
            public Usage usage { get; set; }
        }

        [Serializable]
        public class Choice
        {
            public int index { get; set; }
            public Message message { get; set; }
            public object logprobs { get; set; }
            public string finish_reason { get; set; }

            public override string ToString()
            {
                return message?.content;
            }
        }

        [Serializable]
        public class StreamChoice
        {
            public int index { get; set; }
            public Message delta { get; set; }
            public object logprobs { get; set; }
            public string finish_reason { get; set; }

            public override string ToString()
            {
                return delta?.content;
            }
        }

        [Serializable]
        public class Message
        {
            public string role { get; set; }
            public string content { get; set; }
        }

        [Serializable]
        public class Usage
        {
            public int prompt_tokens { get; set; }
            public int completion_tokens { get; set; }
            public int total_tokens { get; set; }
        }

        [Serializable]
        public class chatRequest
        {
            public string model { get; set; }
            public bool stream { get; set; }

            public List<Message> messages { get; set; }
            // public int max_tokens { get; set; } = 4096;
            // public List<string> stop { get; set; } = new();
            // public float frequency_penalty { get; set; } = 0.0f;
            // public float presence_penalty { get; set; } = 0.0f;
            // public float temperature { get; set; } = 1f;
            // public float top_p { get; set; } = 0.7f;
            // public bool logprobs { get; set; } = false;
            // public int top_logprobs { get; set; } = 0;
            // public Dictionary<int, float> logit_bias { get; set; } = new();

            public chatRequest(string model, string prompt)
            {
                this.model = model;
                this.messages = new List<Message>
                {
                    new() { role = "user", content = prompt }
                };
            }
        }
    }
}