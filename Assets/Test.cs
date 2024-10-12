using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using YuoTools.Extend.Helper;

public class Test : MonoBehaviour
{
    [Button]
    private void Run()
    {
        // OpenAPI 规范的 JSON 字符串
        // JSONPlaceholder API 规范
        string apiSpecJson = @"{
            ""openapi"": ""3.0.0"",
            ""info"": {
                ""title"": ""JSONPlaceholder"",
                ""version"": ""1.0.0""
            },
            ""servers"": [
                {
                    ""url"": ""https://jsonplaceholder.typicode.com""
                }
            ],
            ""paths"": {
                ""/posts"": {
                    ""get"": {
                        ""summary"": ""Get all posts""
                    }
                },
                ""/posts/{id}"": {
                    ""get"": {
                        ""summary"": ""Get a post by ID""
                    }
                }
            }
        }";

        OpenAPIHelper.Initialize(apiSpecJson);

        // 调用 API
        GetAllPosts();
    }

    private async void GetAllPosts()
    {
        string response = await OpenAPIHelper.CallAPI("/posts", "GET");
        Debug.Log("All Posts: " + response);
    }

    private async void GetPost(int id)
    {
        string response = await OpenAPIHelper.CallAPI($"/posts/{id}", "GET");
        Debug.Log($"Post {id}: " + response);
    }
}