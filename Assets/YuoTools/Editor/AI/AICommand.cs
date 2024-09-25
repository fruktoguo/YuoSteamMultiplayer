using UnityEngine;
using UnityEditor;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using YuoTools.Extend.Helper;

namespace YuoTools.Extend.AI
{
    public sealed class AICommandWindow : OdinEditorWindow
    {
        #region Temporary script file operations

        const string TempFilePath = "Assets/AICommandTemp.cs";

        bool TempFileExists => System.IO.File.Exists(TempFilePath);

        void CreateScriptAsset(string code)
        {
            // UnityEditor internal method: ProjectWindowUtil.CreateScriptAssetWithContent
            var flags = BindingFlags.Static | BindingFlags.NonPublic;
            var method = typeof(ProjectWindowUtil).GetMethod("CreateScriptAssetWithContent", flags);
            if (method != null) method.Invoke(null, new object[] { TempFilePath, code });
        }

        #endregion

        #region Script generator

        [EnumToggleButtons] public CommandType commandType = CommandType.EditorScript;

        public enum CommandType
        {
            EditorScript,
            CmdCommand,
        }

        static string EditorScriptPrompt(string input)
            => "编写一个Unity编辑器脚本。\n" +
               " - 它的功能作为一个菜单项放置在\"Edit\" > \"Do Task\"。\n" +
               " - 它不提供任何编辑器窗口。当菜单项被调用时，它会立即执行任务。\n" +
               " - 不要使用GameObject.FindGameObjectsWithTag。\n" +
               " - 没有选中的对象。手动查找游戏对象。\n" +
               "需要完整限定命名空间。\n" +
               "你绝对不能添加除了代码和注释以外的任何解释\n" +
               "如果非要解释,则必须全部使用双斜杠的注释来修饰\n" +
               input;

        static string CmdPrompt(string input)
            => "编写一个可以在Windows命令行中执行的命令。\n" +
               "任务描述如下：\n" +
               "不需要任何的注释\n" +
               "这个命令必须要在Windows命令行中执行\n" +
               "必须保证不会严重威胁Windows安全\n" +
               "你绝对不能添加除了代码以外的任何解释\n" +
               input;

        [HorizontalGroup("button")]
        [Button("生成代码", ButtonHeight = 100)]
        async void CreateGenerator()
        {
            var message = commandType switch
            {
                CommandType.EditorScript => EditorScriptPrompt(prompt),
                CommandType.CmdCommand => CmdPrompt(prompt),
                _ => ""
            };

            await foreach (var line in AIHelper.GenerateTextStream(message))
            {
                result = line;
            }
        }

        [HorizontalGroup("button")]
        [Button("运行代码", ButtonHeight = 100)]
        void RunGenerator()
        {
            if (result.IsNullOrWhitespace()) return;
            switch (commandType)
            {
                case CommandType.EditorScript:
                    CreateScriptAsset(result);
                    break;
#if UNITY_EDITOR_WIN
                case CommandType.CmdCommand:
                    WindowsHelper.Command(result);
                    break;
#endif
            }
        }

        #endregion

        #region Editor GUI

        [HorizontalGroup()] [TextArea(5, 100)] [LabelWidth(100)]
        public string prompt = "输入Command";

        [HorizontalGroup()] [TextArea(5, 100)] public string result = "";


        [MenuItem("Tools/YuoTools/AI Command")]
        static void Init() => GetWindow<AICommandWindow>(true, "AI Command");

        #endregion

        #region Script lifecycle

        protected override void OnEnable()
            => AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;

        protected override void OnDisable()
            => AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;

        void OnAfterAssemblyReload()
        {
            if (!TempFileExists) return;
            EditorApplication.ExecuteMenuItem("Edit/Do Task");
            AssetDatabase.DeleteAsset(TempFilePath);
        }

        #endregion
    }
} // namespace AICommand