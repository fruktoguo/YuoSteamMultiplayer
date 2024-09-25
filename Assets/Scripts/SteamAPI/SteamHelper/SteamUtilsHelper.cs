using System;
using Steamworks;
using UnityEngine;

namespace SteamAPI.SteamHelper
{
    public class SteamUtilsHelper
    {
        /// <summary>
        /// <para> 返回自用户活跃以来的秒数</para>
        /// </summary>
        public static uint GetSecondsSinceAppActive() => SteamUtils.GetSecondsSinceAppActive();

        /// <summary>
        /// <para> 返回自计算机活跃以来的秒数</para>
        /// </summary>
        public static uint GetSecondsSinceComputerActive() => SteamUtils.GetSecondsSinceComputerActive();

        /// <summary>
        /// <para> 返回客户端连接的宇宙</para>
        /// </summary>
        public static EUniverse GetConnectedUniverse() => SteamUtils.GetConnectedUniverse();

        /// <summary>
        /// <para> 返回Steam服务器时间。自1970年1月1日以来的秒数，GMT（即Unix时间）</para>
        /// </summary>
        public static uint GetServerRealTime() => SteamUtils.GetServerRealTime();

        /// <summary>
        /// <para> 返回客户端运行所在国家的2位ISO 3166-1-alpha-2格式国家代码（通过IP到位置数据库查找）</para>
        /// <para> 例如 "US" 或 "UK"。</para>
        /// </summary>
        public static string GetIPCountry() => SteamUtils.GetIPCountry();

        /// <summary>
        /// <para> 如果图像存在并且填充了有效尺寸，则返回true</para>
        /// </summary>
        public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight) =>
            SteamUtils.GetImageSize(iImage, out pnWidth, out pnHeight);

        /// <summary>
        /// <para> 如果图像存在并且成功填充了缓冲区，则返回true</para>
        /// <para> 结果以RGBA格式返回</para>
        /// <para> 目标缓冲区大小应为4 * 高度 * 宽度 * sizeof(char)</para>
        /// </summary>
        public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize) =>
            SteamUtils.GetImageRGBA(iImage, pubDest, nDestBufferSize);

        /// <summary>
        /// <para> 返回当前系统剩余的电池电量百分比 [0..100]，255表示使用交流电</para>
        /// </summary>
        public static byte GetCurrentBatteryPower() => SteamUtils.GetCurrentBatteryPower();

        /// <summary>
        /// <para> 返回当前进程的appID</para>
        /// </summary>
        public static AppId_t GetAppID() => SteamUtils.GetAppID();

        /// <summary>
        /// <para> 设置当前调用游戏的覆盖实例应显示通知的位置。</para>
        /// <para> 该位置是每个游戏的，如果在游戏上下文之外调用此函数，则不会执行任何操作。</para>
        /// </summary>
        public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition) =>
            SteamUtils.SetOverlayNotificationPosition(eNotificationPosition);

        /// <summary>
        /// <para> 返回Steam客户端的语言，您可能更希望使用ISteamApps::GetCurrentGameLanguage，这仅用于非常特殊的使用情况</para>
        /// </summary>
        public static string GetSteamUILanguage() => SteamUtils.GetSteamUILanguage();

        /// <summary>
        /// <para> 返回true，如果Steam本身在VR模式下运行</para>
        /// </summary>
        public static bool IsSteamRunningInVR() => SteamUtils.IsSteamRunningInVR();

        /// <summary>
        /// <para> 设置覆盖通知从SetOverlayNotificationPosition指定的角落的插入位置。</para>
        /// </summary>
        public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset) =>
            SteamUtils.SetOverlayNotificationInset(nHorizontalInset, nVerticalInset);

        /// <summary>
        /// <para> 返回true，如果Steam和Steam覆盖在大屏幕模式下运行</para>
        /// <para> 游戏必须通过Steam客户端启动才能启用大屏幕覆盖。在开发过程中，</para>
        /// <para> 可以将游戏添加为开发者库中的非Steam游戏以测试此功能</para>
        /// </summary>
        public static bool IsSteamInBigPictureMode() => SteamUtils.IsSteamInBigPictureMode();

        /// <summary>
        /// <para> 请求SteamUI创建并渲染其OpenVR仪表板</para>
        /// </summary>
        public static void StartVRDashboard() => SteamUtils.StartVRDashboard();

        /// <summary>
        /// <para> 返回先前输入的文本和长度</para>
        /// </summary>
        public static uint GetEnteredGamepadTextLength() => SteamUtils.GetEnteredGamepadTextLength();

        /// <summary>
        /// <para> 返回先前输入的文本</para>
        /// </summary>
        public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText) =>
            SteamUtils.GetEnteredGamepadTextInput(out pchText, cchText);

        /// <summary>
        /// <para> 激活全屏文本输入对话框，该对话框接受初始文本字符串并返回用户输入的文本</para>
        /// </summary>
        public static bool ShowGamepadTextInput(EGamepadTextInputMode eInputMode,
            EGamepadTextInputLineMode eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText) =>
            SteamUtils.ShowGamepadTextInput(eInputMode, eLineInputMode, pchDescription, unCharMax, pchExistingText);

        /// <summary>
        /// <para> 检查可执行文件是否已使用合作伙伴网站签名选项卡上设置的公钥签名，例如拒绝加载修改后的可执行文件。</para>
        /// <para> 结果在CheckFileSignature_t中返回。</para>
        /// <para> k_ECheckFileSignatureNoSignaturesFoundForThisApp - 此应用未在合作伙伴网站的签名选项卡上配置以启用此功能。</para>
        /// <para> k_ECheckFileSignatureNoSignaturesFoundForThisFile - 此文件未在合作伙伴网站的签名选项卡上列出。</para>
        /// <para> k_ECheckFileSignatureFileNotFound - 该文件在磁盘上不存在。</para>
        /// <para> k_ECheckFileSignatureInvalidSignature - 文件存在，并且为此文件设置了签名选项卡，但文件未签名或签名不匹配。</para>
        /// <para> k_ECheckFileSignatureValidSignature - 文件已签名且签名有效。</para>
        /// </summary>
        public static SteamAPICall_t CheckFileSignature(string szFileName) => SteamUtils.CheckFileSignature(szFileName);

        /// <summary>
        /// <para> 返回true，如果覆盖正在运行并且用户可以访问它。覆盖进程可能需要几秒钟才能启动并挂钩游戏进程，因此在覆盖加载时此函数最初将返回false。</para>
        /// </summary>
        public static bool IsOverlayEnabled() => SteamUtils.IsOverlayEnabled();

        /// <summary>
        /// <para> 设置警告消息钩子</para>
        /// <para> 'int' 是严重性；0表示消息，1表示警告</para>
        /// <para> 'const char *' 是消息的文本</para>
        /// <para> 回调将在生成警告或消息的API函数调用之后直接发生</para>
        /// </summary>
        public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction) =>
            SteamUtils.SetWarningMessageHook(pFunction);

        /// <summary>
        /// <para> 返回自上次调用此函数以来进行的IPC调用次数</para>
        /// <para> 用于性能调试，以便您了解每帧游戏进行的IPC调用次数</para>
        /// <para> 每个IPC调用至少是一个线程上下文切换，如果不是进程切换，因此您希望控制它们的频率。</para>
        /// </summary>
        public static uint GetIPCCallCount() => SteamUtils.GetIPCCallCount();

        /// <summary>
        /// <para> 返回API异步调用结果</para>
        /// <para> 可以直接使用，但更常见的是通过回调分派API使用（参见steam_api.h）</para>
        /// </summary>
        public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed) =>
            SteamUtils.IsAPICallCompleted(hSteamAPICall, out pbFailed);

        /// <summary>
        /// <para> 返回API调用失败的原因</para>
        /// </summary>
        public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall) =>
            SteamUtils.GetAPICallFailureReason(hSteamAPICall);

        /// <summary>
        /// <para> 返回API调用结果</para>
        /// </summary>
        public static bool GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback,
            int iCallbackExpected, out bool pbFailed) => SteamUtils.GetAPICallResult(hSteamAPICall, pCallback,
            cubCallback, iCallbackExpected, out pbFailed);

        /// <summary>
        /// <para> 返回当前Steam客户端是否为Steam中国特定客户端，与全球客户端相比。</para>
        /// </summary>
        public static bool IsSteamChinaLauncher() => SteamUtils.IsSteamChinaLauncher();

        /// <summary>
        /// <para> 初始化文本过滤，加载游戏运行语言的词典。</para>
        /// <para> unFilterOptions保留供将来使用，应设置为0</para>
        /// <para> 如果过滤器不可用于游戏的语言，则返回false，在这种情况下，FilterText()将作为直通。</para>
        /// <para> 用户可以在其Steam账户首选项中自定义文本过滤行为：</para>
        /// <para> https://store.steampowered.com/account/preferences#CommunityContentPreferences</para>
        /// </summary>
        public static bool InitFilterText(uint unFilterOptions = 0) => SteamUtils.InitFilterText(unFilterOptions);

        /// <summary>
        /// <para> 过滤提供的输入消息，并将过滤结果放入pchOutFilteredText中，使用法律要求的过滤和基于上下文和用户设置的附加过滤</para>
        /// <para> eContext是输入字符串中的内容类型</para>
        /// <para> sourceSteamID是输入字符串的来源Steam ID（例如，具有名称的玩家或说出聊天文本的玩家）</para>
        /// <para> pchInputText是应过滤的输入字符串，可以是ASCII或UTF-8</para>
        /// <para> pchOutFilteredText是输出将放置的位置，即使没有执行过滤</para>
        /// <para> nByteSizeOutFilteredText是pchOutFilteredText的大小（以字节为单位），应至少为strlen(pchInputText)+1</para>
        /// <para> 返回过滤的字符数（不是字节数）</para>
        /// </summary>
        public static int FilterText(ETextFilteringContext eContext, CSteamID sourceSteamID, string pchInputMessage,
            out string pchOutFilteredText, uint nByteSizeOutFilteredText) => SteamUtils.FilterText(eContext,
            sourceSteamID, pchInputMessage, out pchOutFilteredText, nByteSizeOutFilteredText);

        /// <summary>
        /// <para> 返回我们认为您当前在指定协议上的“互联网”IPv6连接状态。</para>
        /// <para> 这不会告诉您Steam客户端当前是否通过IPv6连接到Steam。</para>
        /// </summary>
        public static ESteamIPv6ConnectivityState GetIPv6ConnectivityState(ESteamIPv6ConnectivityProtocol eProtocol) =>
            SteamUtils.GetIPv6ConnectivityState(eProtocol);

        /// <summary>
        /// <para> 返回true，如果当前在Steam Deck设备上运行</para>
        /// </summary>
        public static bool IsSteamRunningOnSteamDeck() => SteamUtils.IsSteamRunningOnSteamDeck();

        /// <summary>
        /// <para> 在游戏内容上打开浮动键盘，并将操作系统键盘键直接发送到游戏。</para>
        /// <para> 文本字段位置以相对于游戏窗口原点的像素为单位指定，用于将浮动键盘定位在不覆盖文本字段的位置</para>
        /// </summary>
        public static bool ShowFloatingGamepadTextInput(EFloatingGamepadTextInputMode eKeyboardMode,
            int nTextFieldXPosition, int nTextFieldYPosition, int nTextFieldWidth, int nTextFieldHeight) =>
            SteamUtils.ShowFloatingGamepadTextInput(eKeyboardMode, nTextFieldXPosition, nTextFieldYPosition,
                nTextFieldWidth, nTextFieldHeight);

        /// <summary>
        /// <para> 在没有控制器支持的游戏启动器中，您可以调用此函数以使Steam Input将控制器输入转换为鼠标/键盘以导航启动器</para>
        /// </summary>
        public static void SetGameLauncherMode(bool bLauncherMode) => SteamUtils.SetGameLauncherMode(bLauncherMode);

        /// <summary>
        /// <para> 关闭浮动键盘。</para>
        /// </summary>
        public static bool DismissFloatingGamepadTextInput() => SteamUtils.DismissFloatingGamepadTextInput();

        /// <summary>
        /// <para> 关闭全屏文本输入对话框。</para>
        /// </summary>
        public static bool DismissGamepadTextInput() => SteamUtils.DismissGamepadTextInput();

        /// <summary>
        /// <para> 返回true，如果HMD内容将通过Steam远程播放流式传输</para>
        /// </summary>
        public static bool IsVRHeadsetStreamingEnabled() => SteamUtils.IsVRHeadsetStreamingEnabled();

        /// <summary>
        /// <para> 设置HMD内容是否将通过Steam远程播放流式传输</para>
        /// <para> 如果设置为true，则HMD耳机中的场景将被流式传输，并且不允许远程输入。</para>
        /// <para> 如果设置为false，则应用程序窗口将被流式传输，并且允许远程输入。</para>
        /// <para> 默认情况下为true，除非游戏的扩展appinfo中有“VRHeadsetStreaming” “0”。</para>
        /// <para> （这对于具有不对称多人游戏的游戏很有用）</para>
        /// </summary>
        public static void SetVRHeadsetStreamingEnabled(bool bEnabled) =>
            SteamUtils.SetVRHeadsetStreamingEnabled(bEnabled);

        public static Texture2D GetImage(int image)
        {
            if (image == -1 || image == 0)
                return null;

            if (!SteamUtils.GetImageSize(image, out uint width, out uint height))
                return null;

            uint length = width * height * 4;
            byte[] buffer = new byte[length];

            if (!SteamUtils.GetImageRGBA(image, buffer, (int)length))
                return null;

            // 创建一个新的缓冲区来存储翻转后的图像数据
            byte[] flippedBuffer = new byte[length];

            // 翻转图像数据
            for (uint y = 0; y < height; y++)
            {
                for (uint x = 0; x < width; x++)
                {
                    uint srcIndex = (y * width + x) * 4;
                    uint dstIndex = ((height - 1 - y) * width + x) * 4;
                    flippedBuffer[dstIndex] = buffer[srcIndex];
                    flippedBuffer[dstIndex + 1] = buffer[srcIndex + 1];
                    flippedBuffer[dstIndex + 2] = buffer[srcIndex + 2];
                    flippedBuffer[dstIndex + 3] = buffer[srcIndex + 3];
                }
            }

            Texture2D texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
            texture.LoadRawTextureData(flippedBuffer);
            texture.Apply();

            return texture;
        }
    }
}