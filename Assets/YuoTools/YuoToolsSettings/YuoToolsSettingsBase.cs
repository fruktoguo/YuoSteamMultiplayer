using UnityEngine;
using System.IO;

public partial class YuoToolsSettings : ScriptableObject
{
    // 配置文件存放路径
    private static string GetSettingsPath()
    {
        return $"ProjectSettings/YuoToolsSettings.json";
    }

    // 获取或创建设置文件实例
    public static YuoToolsSettings GetOrCreateSettings()
    {
#if UNITY_EDITOR
        string path = GetSettingsPath();
        YuoToolsSettings settings;
        if (File.Exists(path))
        {
            // 如果文件存在，从JSON文件中加载
            settings = ScriptableObject.CreateInstance<YuoToolsSettings>();
            JsonUtility.FromJsonOverwrite(File.ReadAllText(path), settings);
        }
        else
        {
            // 如果文件不存在，创建一个新的实例
            settings = CreateInstance<YuoToolsSettings>();
            SaveSettings(settings);
        }

        SaveResource(settings);

        return settings;
#else
        return Resources.Load<YuoToolsSettings>("YuoToolsSettings");
#endif
    }

    static void SaveResource(YuoToolsSettings settings)
    {
#if UNITY_EDITOR
        var path = "Assets/Resources/YuoToolsSettings.asset";
        //保存到Resources文件夹
        //如果文件不存在就创建,存在则覆盖
        if (File.Exists(path))
        {
            UnityEditor.AssetDatabase.DeleteAsset(path);
        }

        UnityEditor.AssetDatabase.CreateAsset(settings, path);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

    // 保存设置到文件
    public static void SaveSettings(YuoToolsSettings settings)
    {
        string path = GetSettingsPath();
        File.WriteAllText(path, JsonUtility.ToJson(settings, true));
        SaveResource(settings);
    }
}