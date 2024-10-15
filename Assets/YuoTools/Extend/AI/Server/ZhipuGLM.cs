using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace YuoTools.Extend.AI
{
    public class ZhipuGLM
    {
        public const string URL = "https://open.bigmodel.cn/api/paas/v4/chat/completions";

        public static async Task<string> GenerateText(string prompt)
        {
            var request = new ChatCompletionRequest
            {
                model = AIHelper.AIModel,
                messages = new List<ChatCompletionRequest.Message>
                {
                    new()
                    {
                        role = "user",
                        content = prompt
                    }
                },
                stream = false
            };

            var body = JsonConvert.SerializeObject(request);
            var responseJson = await AIServerHelper.GetResponse(AIHelper.ApiKey, URL, body);

            if (string.IsNullOrEmpty(responseJson)) return "发生错误";
            var parsedJson = JsonConvert.DeserializeObject<ChatCompletionResponse>(responseJson);
            return parsedJson.choices[0].message.content;
        }

        public static async IAsyncEnumerable<string> GenerateStream(string prompt)
        {
            var request = new ChatCompletionRequest
            {
                model = AIHelper.AIModel,
                messages = new List<ChatCompletionRequest.Message>
                {
                    new()
                    {
                        role = "user",
                        content = prompt
                    }
                },
                stream = true
            };

            var body = JsonConvert.SerializeObject(request);
            
            string OnJson(ChatCompletionStreamResponse json)
            {
                return json.choices[0].delta.content;
            }

            string OnText(string text)
            {
                return OperateMessage(text, OnJson);
            }
            
            await foreach (var line in AIServerHelper.GetResponseStream(AIHelper.ApiKey, URL, body, OnText))
            {
                if (line != null) yield return line;
            }
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
        public class ChatCompletionRequest
        {
            public string model; // 要调用的模型编码
            public List<Message> messages; // 对话消息列表
            public bool stream = false; // 是否使用流式输出，默认false

            [Serializable]
            public class Message
            {
                public string role; // 消息角色：system, user, assistant, tool
                public string content; // 消息内容
                public List<ToolCall> tool_calls; // 工具调用信息
                public string tool_call_id; // 工具调用的ID，用于Tool Message
            }

            #region 非必须

            public string request_id; // 请求的唯一标识符
            public string tool_choice; // 控制模型选择调用哪个函数的方式，默认auto
            public bool do_sample = true; // 是否启用采样策略，默认true
            public float temperature = 0.95f; // 采样温度，默认0.95
            public float top_p = 0.7f; // 核采样方法，默认0.7
            public int max_tokens = 1024; // 模型输出的最大token数，默认1024
            public List<string> stop; // 模型停止生成的停止词
            public List<Tool> tools; // 模型可以调用的工具
            public string user_id; // 终端用户的唯一ID

            [Serializable]
            public class ToolCall
            {
                public string id; // 工具调用的唯一标识符
                public string type; // 工具类型，目前仅支持'function'
                public Function function; // 函数调用信息
            }

            [Serializable]
            public class Function
            {
                public string name; // 函数名称
                public string arguments; // 函数参数，JSON格式
            }

            [Serializable]
            public class Tool
            {
                public string type; // 工具类型：function, retrieval, web_search
                public FunctionDefinition function; // 函数定义
                public RetrievalDefinition retrieval; // 检索工具定义
                public WebSearchDefinition web_search; // 网络搜索工具定义
            }

            [Serializable]
            public class FunctionDefinition
            {
                public string name; // 函数名称
                public string description; // 函数描述
                public ParametersDefinition parameters; // 函数参数定义
            }

            [Serializable]
            public class ParametersDefinition
            {
                public string type; // 参数类型，通常为"object"
                public Dictionary<string, PropertyDefinition> properties; // 参数属性定义
                public List<string> required; // 必需的参数列表
            }

            [Serializable]
            public class PropertyDefinition
            {
                public string type; // 属性类型
                public string description; // 属性描述
                public List<string> enum_values; // 枚举值列表（如果适用）
            }

            [Serializable]
            public class RetrievalDefinition
            {
                public string knowledge_id; // 知识库ID
                public string prompt_template; // 知识库模板
            }

            [Serializable]
            public class WebSearchDefinition
            {
                public bool enable; // 是否启用网络搜索
                public string search_query; // 自定义搜索查询
                public bool search_result; // 是否获取网页搜索来源的详细信息
            }

            #endregion
        }

        [Serializable]
        public class ChatCompletionResponse
        {
            public string id; // 任务ID
            public string request_id; // 请求ID
            public long created; // 请求创建时间（Unix时间戳）
            public string model; // 使用的模型名称
            public List<Choice> choices; // 模型输出内容
            public Usage usage; // token使用统计
            public List<WebSearch> web_search; // 网页搜索结果（如果适用）
            public string task_status; // 任务状态（用于异步调用）

            [Serializable]
            public class Choice
            {
                public int index; // 结果索引
                public string finish_reason; // 生成终止原因
                public Message message; // 模型返回的消息
            }

            [Serializable]
            public class Message
            {
                public string role; // 消息角色
                public string content; // 消息内容
                public List<ToolCall> tool_calls; // 工具调用信息
            }

            [Serializable]
            public class ToolCall
            {
                public string id; // 工具调用的唯一标识符
                public string type; // 工具类型
                public Function function; // 函数调用信息
            }

            [Serializable]
            public class Function
            {
                public string name; // 函数名称
                public string arguments; // 函数参数，JSON格式
            }

            [Serializable]
            public class Usage
            {
                public int prompt_tokens; // 用户输入的token数量
                public int completion_tokens; // 模型输出的token数量
                public int total_tokens; // 总token数量
            }

            [Serializable]
            public class WebSearch
            {
                public string icon; // 来源网站的图标
                public string title; // 搜索结果的标题
                public string link; // 搜索结果的网页链接
                public string media; // 搜索结果网页的媒体来源名称
                public string content; // 搜索结果网页引用的文本内容
            }
        }

        public class ChatCompletionStreamResponse
        {
            public string id { get; set; }
            public string request_id { get; set; }
            public long created { get; set; }
            public string model { get; set; }
            public List<Choice> choices { get; set; }
            public Usage usage { get; set; }

            [Serializable]
            public class Choice
            {
                public int index { get; set; }
                public Delta delta { get; set; }
                public string finish_reason { get; set; }
            }

            [Serializable]
            public class Delta
            {
                public string role { get; set; }
                public string content { get; set; }
                public List<ToolCall> tool_calls { get; set; }
            }

            [Serializable]
            public class ToolCall
            {
                public string id { get; set; }
                public string type { get; set; }
                public Function function { get; set; }
            }

            [Serializable]
            public class Function
            {
                public string name { get; set; }
                public string arguments { get; set; }
            }

            [Serializable]
            public class Usage
            {
                public int prompt_tokens { get; set; }
                public int completion_tokens { get; set; }
                public int total_tokens { get; set; }
            }
        }

        public class ChatCompletionStreamFinishResponse
        {
            public string id { get; set; }
            public string request_id { get; set; }
            public string @object { get; set; }
            public long created { get; set; }
            public string model { get; set; }
            public List<Choice> choices { get; set; }
            public Usage usage { get; set; }

            [Serializable]
            public class Choice
            {
                public int index { get; set; }
                public string finish_reason { get; set; }
            }

            [Serializable]
            public class Usage
            {
                public int prompt_tokens { get; set; }
                public int completion_tokens { get; set; }
                public int total_tokens { get; set; }
            }
        }
    }
}