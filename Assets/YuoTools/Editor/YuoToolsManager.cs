using System.Globalization;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using YuoTools.Extend.Helper;
using System.Threading.Tasks;
using System.Threading;

#if UNITY_EDITOR_WIN
namespace YuoTools.Editor
{
    public class YuoToolsManager : OdinEditorWindow
    {
        [MenuItem("Tools/YuoTools Manager")]
        private static void OpenWindow()
        {
            var window = GetWindow<YuoToolsManager>();
            window.titleContent = new GUIContent("YuoTools Manager");
            window.minSize = new Vector2(620, 100);
            window.maxSize = new Vector2(620, 100);
            window.Show();
        }

        [ShowInInspector] public string LastWriteTime => GetLastWriteTime();
        [ShowInInspector] public string FolderSize => GetCachedFolderSize();

        public const string Path = @"C:\YuoTools\Main";
        public const string BackupPath = @"C:\YuoTools\Backup";

        string LocalPath => $"{Application.dataPath}/YuoTools";

        private string _cachedFolderSize;
        private long _lastWriteTime;

        string GetLastWriteTime()
        {
            var time = "没有文件夹";
            if (Directory.Exists(Path))
            {
                var t = GetLastWriteTime(Path);
                if (t > 0)
                {
                    var date = new System.DateTime(t);
                    time = date.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            return time;
        }

        string GetFolderSize()
        {
            if (!Directory.Exists(Path))
            {
                return "没有文件夹";
            }

            long size = Directory.GetFiles(Path, "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t)).Length);
            return FormatSize(size);
        }

        string FormatSize(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = size;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        string GetCachedFolderSize()
        {
            var currentWriteTime = GetLastWriteTime(Path);
            if (currentWriteTime != _lastWriteTime)
            {
                _lastWriteTime = currentWriteTime;
                _cachedFolderSize = GetFolderSize();
            }

            return _cachedFolderSize;
        }

        [HorizontalGroup("Buttons")]
        [GUIColor(0.4f, 0.8f, 1.0f)]
        [Button("上传", ButtonSizes.Large)]
        public async void Upload()
        {
            EditorUtility.DisplayProgressBar("上传", "正在上传文件...", 0.0f);
            var cts = new CancellationTokenSource();
            var timeoutTask = Task.Delay(30000, cts.Token); // 30秒超时
            var uploadTask = Task.Run(() =>
            {
                FileHelper.CheckOrCreateDirectoryPath(Path);
                var time = System.DateTime.Now.Ticks;
                WriteTime(LocalPath, time);
                FileHelper.CleanDirectory(Path);
                FileHelper.CopyDirectory(LocalPath, Path);
                WindowsHelper.OpenDirectory(Path);
            });

            float progress = 0.0f;
            while (!uploadTask.IsCompleted && !timeoutTask.IsCompleted)
            {
                progress += 0.01f;
                EditorUtility.DisplayProgressBar("上传", "正在上传文件...", progress);
                await Task.Delay(100); // 每0.1秒更新一次进度条
            }

            var completedTask = await Task.WhenAny(uploadTask, timeoutTask);
            if (completedTask == timeoutTask)
            {
                Debug.LogError("上传操作超时");
            }
            else
            {
                cts.Cancel(); // 取消超时任务
            }

            EditorUtility.ClearProgressBar();

            _cachedFolderSize = GetFolderSize();
        }

        [HorizontalGroup("Buttons")]
        [GUIColor(0.4f, 1.0f, 0.4f)]
        [Button("下载", ButtonSizes.Large)]
        public async void Download()
        {
            var sourceTime = GetLastWriteTime(Path);
            if (sourceTime == 0)
            {
                "没有找到YuoTools文件夹".Log();
                return;
            }

            var localTime = GetLastWriteTime(LocalPath);
            if (sourceTime == localTime)
            {
                var isConfirm =
                    await UnityEditorHelper.ShowConfirmationDialog("提示", "本地YuoTools文件夹已经是最新版本,确认重新下载？", "覆盖", "取消");
                if (!isConfirm)
                {
                    return;
                }
            }
            else
            {
                var isConfirm = await UnityEditorHelper.ShowConfirmationDialog("提示", "是否下载最新YuoTools文件夹？", "覆盖", "取消");
                if (!isConfirm)
                {
                    return;
                }
            }

            EditorUtility.DisplayProgressBar("下载", "正在下载文件...", 0.0f);
            var cts = new CancellationTokenSource();
            var timeoutTask = Task.Delay(30000, cts.Token); // 30秒超时
            var downloadTask = Task.Run(() =>
            {
                FileHelper.CheckOrCreateDirectoryPath(BackupPath);
                FileHelper.CleanDirectory(BackupPath);
                FileHelper.CopyDirectory(LocalPath, BackupPath);
                FileHelper.CleanDirectory(LocalPath);
                FileHelper.CopyDirectory(Path, LocalPath);
                return Task.CompletedTask;
            });

            float progress = 0.0f;
            while (!downloadTask.IsCompleted && !timeoutTask.IsCompleted)
            {
                progress += 0.01f;
                EditorUtility.DisplayProgressBar("下载", "正在下载文件...", progress);
                await Task.Delay(100); // 每0.1秒更新一次进度条
            }

            var completedTask = await Task.WhenAny(downloadTask, timeoutTask);
            if (completedTask == timeoutTask)
            {
                Debug.LogError("下载操作超时");
            }
            else
            {
                cts.Cancel(); // 取消超时任务
            }

            EditorUtility.ClearProgressBar();
        }

        const string WriteTimePath = "WriteTime.txt";

        long GetLastWriteTime(string path)
        {
            if (!Directory.Exists(path))
            {
                return 0;
            }

            var readmePath = $"{path}/{WriteTimePath}";
            FileHelper.CheckOrCreateFile(readmePath);
            var readme = FileHelper.ReadAllText(readmePath);
            if (string.IsNullOrEmpty(readme))
            {
                return 0;
            }

            return long.Parse(readme);
        }

        void WriteTime(string path, long time)
        {
            var readmePath = $"{path}/{WriteTimePath}";
            FileHelper.CheckOrCreateFile(readmePath);
            FileHelper.WriteAllText(readmePath, time.ToString());
        }
    }
}
#endif