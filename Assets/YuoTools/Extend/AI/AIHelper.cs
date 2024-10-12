using System.Collections.Generic;
using System.Threading.Tasks;

namespace YuoTools.Extend.AI
{
    public static class AIHelper
    {
        public static string ApiKey => YuoToolsSettings.GetOrCreateSettings().AIApiKey;
        public static string AIModel => YuoToolsSettings.GetOrCreateSettings().AIModel;

        public static async Task<string> GenerateText(string prompt)
        {
            switch (YuoToolsSettings.GetOrCreateSettings().AIServer)
            {
                // case "gemini":
                //     return await GeminiApi.GenerateText(prompt);
                case "doubao":
                    return await DoubaoApi.GenerateText(prompt);
                default:
                    return "没有配置正确的服务商";
            }
        }

        public static async IAsyncEnumerable<string> GenerateTextStream(string prompt)
        {
            switch (YuoToolsSettings.GetOrCreateSettings().AIServer)
            {
                // case "gemini":
                //     await foreach (var line in GeminiApi.GenerateStream(prompt))
                //     {
                //         yield return line;
                //     }
                //
                //     break;
                case "doubao":
                    await foreach (var line in DoubaoApi.GenerateStream(prompt))
                    {
                        yield return line;
                    }

                    break;
                default:
                    yield return "没有配置正确的服务商";
                    break;
            }
        }
    }
}