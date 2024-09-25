using Steamworks;

namespace SteamAPI.SteamHelper
{
    public static class SteamUserHelper
    {
        /// <summary>
        /// <para> 获取此接口代表的HSteamUser</para>
        /// <para> 这仅供API内部使用，以及少数支持多用户的接口使用</para>
        /// </summary>
        public static HSteamUser GetHSteamUser() => SteamUser.GetHSteamUser();

        /// <summary>
        /// <para> 检查Steam客户端是否与Steam服务器保持活跃连接。</para>
        /// <para> 如果返回false，表示由于本地机器的网络问题或Steam服务器宕机/繁忙，当前没有活动连接。</para>
        /// <para> Steam客户端将尽可能频繁地自动尝试重新创建连接。</para>
        /// </summary>
        public static bool IsLoggedOn() => SteamUser.BLoggedOn();

        /// <summary>
        /// <para> 获取当前登录Steam客户端的账户的CSteamID</para>
        /// <para> CSteamID是账户的唯一标识符，用于在Steamworks API的所有部分中区分用户</para>
        /// </summary>
        public static CSteamID GetSteamID() => SteamUser.GetSteamID();

        /// <summary>
        /// <para> 多人游戏认证函数</para>
        /// <para> InitiateGameConnection()启动用于认证游戏客户端与游戏服务器的状态机</para>
        /// <para> 它是客户端与游戏服务器和Steam服务器之间的三方握手的一部分</para>
        /// <para> 参数:</para>
        /// <para> void *pAuthBlob - 指向将填充认证令牌的空内存的指针。</para>
        /// <para> int cbMaxAuthBlob - pBlob中分配的内存字节数。应至少为2048字节。</para>
        /// <para> CSteamID steamIDGameServer - 从游戏服务器接收到的游戏服务器的steamID</para>
        /// <para> CGameID gameID - 当前游戏的ID。对于没有mod的游戏，这只是CGameID(&lt;appID&gt;)</para>
        /// <para> uint32 unIPServer, uint16 usPortServer - 游戏服务器的IP地址</para>
        /// <para> bool bSecure - 客户端是否认为游戏服务器报告自己为安全（即VAC正在运行）</para>
        /// <para> 返回值 - 返回写入pBlob的字节数。如果返回值为0，则传入的缓冲区太小，调用失败</para>
        /// <para> pBlob的内容应发送到游戏服务器，以完成认证过程。</para>
        /// <para> 已弃用！此函数将在即将发布的SDK版本中删除。</para>
        /// <para> 请迁移到BeginAuthSession及相关函数。</para>
        /// </summary>
        public static int InitiateGameConnection_DEPRECATED(byte[] pAuthBlob, int cbMaxAuthBlob, CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, bool bSecure) =>
            SteamUser.InitiateGameConnection_DEPRECATED(pAuthBlob, cbMaxAuthBlob, steamIDGameServer, unIPServer, usPortServer, bSecure);

        /// <summary>
        /// <para> 断开连接通知</para>
        /// <para> 当游戏客户端离开指定的游戏服务器时需要发生，需要与InitiateGameConnection()调用匹配</para>
        /// <para> 已弃用！此函数将在即将发布的SDK版本中删除。</para>
        /// <para> 请迁移到BeginAuthSession及相关函数。</para>
        /// </summary>
        public static void TerminateGameConnection_DEPRECATED(uint unIPServer, ushort usPortServer) =>
            SteamUser.TerminateGameConnection_DEPRECATED(unIPServer, usPortServer);

        /// <summary>
        /// <para> 旧版函数</para>
        /// <para> 仅由少数游戏用于跟踪使用事件</para>
        /// </summary>
        public static void TrackAppUsageEvent(CGameID gameID, int eAppUsageEvent, string pchExtraInfo = "") =>
            SteamUser.TrackAppUsageEvent(gameID, eAppUsageEvent, pchExtraInfo);

        /// <summary>
        /// <para> 获取当前Steam账户的本地存储文件夹以写入应用数据，例如保存游戏、配置等。</para>
        /// <para> 这通常是类似于"C:\Progam Files\Steam\userdata\&lt;SteamID&gt;\&lt;AppID&gt;\local"的路径</para>
        /// </summary>
        public static bool GetUserDataFolder(out string pchBuffer, int cubBuffer) =>
            SteamUser.GetUserDataFolder(out pchBuffer, cubBuffer);

        /// <summary>
        /// <para> 开始语音录音。一旦开始，使用GetVoice()获取数据</para>
        /// </summary>
        public static void StartVoiceRecording() => SteamUser.StartVoiceRecording();

        /// <summary>
        /// <para> 停止语音录音。由于人们经常提前释放按键通话键，系统将在调用此函数后继续录音一段时间。</para>
        /// <para> 应继续调用GetVoice()直到返回k_eVoiceResultNotRecording</para>
        /// </summary>
        public static void StopVoiceRecording() => SteamUser.StopVoiceRecording();

        /// <summary>
        /// <para> 获取可用的压缩语音数据大小</para>
        /// <para> 可能已经经过预处理过滤和/或删除了静音，因此未压缩的音频可能比预期的持续时间更短。</para>
        /// <para> 在长时间的静音期间可能没有数据。此外，获取未压缩的音频将导致GetVoice丢弃任何剩余的压缩音频，因此必须同时获取两种类型。</para>
        /// <para> 最后，当请求未压缩大小时，GetAvailableVoice并不完全准确。因此，如果确实需要使用未压缩音频，应该频繁调用GetVoice，并使用两个非常大的（20kb+）输出缓冲区，而不是尝试分配完美大小的缓冲区。</para>
        /// <para> 但大多数应用程序应忽略所有这些细节，并简单地将“未压缩”参数留为空/零。</para>
        /// <para> ---------------------------------------------------------------------------</para>
        /// <para> 从麦克风缓冲区读取捕获的音频数据。应至少每帧调用一次，最好每几毫秒调用一次，以保持麦克风输入延迟尽可能低。</para>
        /// <para> 大多数应用程序将只使用压缩数据，并应为“未压缩”参数传递NULL/零。</para>
        /// </summary>
        public static EVoiceResult GetAvailableVoice(out uint pcbCompressed) =>
            SteamUser.GetAvailableVoice(out pcbCompressed);

        /// <summary>
        /// <para> 获取语音数据</para>
        /// <para> 压缩数据可以由您的应用程序传输，并使用下面的DecompressVoice函数解码为原始数据。</para>
        /// </summary>
        public static EVoiceResult GetVoice(bool bWantCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten) =>
            SteamUser.GetVoice(bWantCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten);

        /// <summary>
        /// <para> 解压缩语音数据</para>
        /// <para> 解码由GetVoice返回的压缩语音数据。输出数据是原始单通道16位PCM音频。解码器支持从11025到48000的任何采样率；有关详细信息，请参阅下面的GetVoiceOptimalSampleRate()。</para>
        /// <para> 如果输出缓冲区不够大，则*nBytesWritten将设置为所需的缓冲区大小，并返回k_EVoiceResultBufferTooSmall。</para>
        /// <para> 建议从20kb缓冲区开始，并根据需要重新分配。</para>
        /// </summary>
        public static EVoiceResult DecompressVoice(byte[] pCompressed, uint cbCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate) =>
            SteamUser.DecompressVoice(pCompressed, cbCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, nDesiredSampleRate);

        /// <summary>
        /// <para> 获取Steam语音解压缩器的最佳采样率</para>
        /// <para> 使用此采样率进行DecompressVoice将执行最少的CPU处理。</para>
        /// <para> 但是，最终的音频质量将取决于音频设备（和/或您的应用程序的音频输出SDK）如何处理较低的采样率。</para>
        /// <para> 您可能会发现，当您忽略此函数并使用音频输出设备的本机采样率（通常为48000或44100）时，您会获得最佳的音频输出质量。</para>
        /// </summary>
        public static uint GetVoiceOptimalSampleRate() => SteamUser.GetVoiceOptimalSampleRate();

        /// <summary>
        /// <para> 获取认证会话票据</para>
        /// <para> pcbTicket检索实际票据的长度。</para>
        /// <para> SteamNetworkingIdentity是一个可选的输入参数，用于保存您要连接的实体的公共IP地址或SteamID</para>
        /// <para> 如果传递了IP地址，Steam将仅允许具有该IP地址的实体使用票据</para>
        /// <para> 如果传递了Steam ID，Steam将仅允许该Steam ID使用票据</para>
        /// <para> 不适用于“ISteamUserAuth\AuthenticateUserTicket” - 它将失败</para>
        /// </summary>
        public static HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket, ref SteamNetworkingIdentity pSteamNetworkingIdentity) =>
            SteamUser.GetAuthSessionTicket(pTicket, cbMaxTicket, out pcbTicket, ref pSteamNetworkingIdentity);

        /// <summary>
        /// <para> 获取用于Web API的认证票据</para>
        /// <para> pchIdentity是一个可选的输入参数，用于标识将发送票据的服务</para>
        /// <para> 票据将在回调GetTicketForWebApiResponse_t中返回</para>
        /// </summary>
        public static HAuthTicket GetAuthTicketForWebApi(string pchIdentity) =>
            SteamUser.GetAuthTicketForWebApi(pchIdentity);

        /// <summary>
        /// <para> 开始认证会话</para>
        /// <para> 验证来自实体steamID的票据，以确保其有效且未被重用</para>
        /// <para> 如果实体离线或取消票据，则注册回调（请参阅ValidateAuthTicketResponse_t回调和EAuthSessionResponse）</para>
        /// </summary>
        public static EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID) =>
            SteamUser.BeginAuthSession(pAuthTicket, cbAuthTicket, steamID);

        /// <summary>
        /// <para> 停止由BeginAuthSession启动的跟踪 - 当不再与该实体一起玩游戏时调用</para>
        /// </summary>
        public static void EndAuthSession(CSteamID steamID) => SteamUser.EndAuthSession(steamID);

        /// <summary>
        /// <para> 取消来自GetAuthSessionTicket的认证票据，当不再与您提供票据的实体一起玩游戏时调用</para>
        /// </summary>
        public static void CancelAuthTicket(HAuthTicket hAuthTicket) => SteamUser.CancelAuthTicket(hAuthTicket);

        /// <summary>
        /// <para> 在接收到用户的认证数据并将其传递给BeginAuthSession后，使用此函数确定用户是否拥有指定AppID的可下载内容。</para>
        /// </summary>
        public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID) =>
            SteamUser.UserHasLicenseForApp(steamID, appID);

        /// <summary>
        /// <para> 返回true，如果此用户看起来在NAT设备后面。仅在用户已连接到Steam后有效（即已发出SteamServersConnected_t），并且可能无法捕获所有形式的NAT。</para>
        /// </summary>
        public static bool IsBehindNAT() => SteamUser.BIsBehindNAT();

        /// <summary>
        /// <para> 设置要复制到朋友的数据，以便他们可以加入您的游戏</para>
        /// <para> CSteamID steamIDGameServer - 从游戏服务器接收到的游戏服务器的steamID</para>
        /// <para> uint32 unIPServer, uint16 usPortServer - 游戏服务器的IP地址</para>
        /// </summary>
        public static void AdvertiseGame(CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer) =>
            SteamUser.AdvertiseGame(steamIDGameServer, unIPServer, usPortServer);

        /// <summary>
        /// <para> 请求使用应用程序特定的共享密钥加密的票据</para>
        /// <para> pDataToInclude, cbDataToInclude将被加密到票据中</para>
        /// <para> （这是异步的，您必须等待服务器完成票据）</para>
        /// </summary>
        public static SteamAPICall_t RequestEncryptedAppTicket(byte[] pDataToInclude, int cbDataToInclude) =>
            SteamUser.RequestEncryptedAppTicket(pDataToInclude, cbDataToInclude);

        /// <summary>
        /// <para> 获取已完成的票据。</para>
        /// <para> 如果没有可用的票据，或者您的缓冲区太小，则返回false。</para>
        /// <para> 退出时，*pcbTicket将是复制到缓冲区中的票据大小（如果返回true），或所需的大小（如果返回false）。要确定票据的正确大小，可以传递pTicket=NULL和cbMaxTicket=0；如果有票据可用，*pcbTicket将包含所需的大小，否则为零。</para>
        /// </summary>
        public static bool GetEncryptedAppTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket) =>
            SteamUser.GetEncryptedAppTicket(pTicket, cbMaxTicket, out pcbTicket);

        /// <summary>
        /// <para> 交易卡徽章数据访问</para>
        /// <para> 如果您只有一套卡片，系列将是1</para>
        /// <para> 用户可以拥有两个不同的系列徽章；常规（最高等级5）和箔（最高等级1）</para>
        /// </summary>
        public static int GetGameBadgeLevel(int nSeries, bool bFoil) =>
            SteamUser.GetGameBadgeLevel(nSeries, bFoil);

        /// <summary>
        /// <para> 获取玩家的Steam等级，如其个人资料中所示</para>
        /// </summary>
        public static int GetPlayerSteamLevel() => SteamUser.GetPlayerSteamLevel();

        /// <summary>
        /// <para> 请求一个URL，该URL将验证游戏内浏览器以进行商店结账，然后重定向到指定的URL。只要游戏内浏览器接受并处理会话cookie，Steam微交易结账页面将自动识别用户，而不是显示登录页面。</para>
        /// <para> 此API调用的结果将是StoreAuthURLResponse_t回调。</para>
        /// <para> 注意：URL的寿命非常短，以防止历史记录窥探攻击，因此您应该只在即将启动浏览器时调用此API，或者立即使用隐藏的浏览器窗口导航到结果URL。</para>
        /// <para> 注意2：生成的授权cookie的过期时间为一天，因此最好每12小时请求并访问一个新的授权URL。</para>
        /// </summary>
        public static SteamAPICall_t RequestStoreAuthURL(string pchRedirectURL) =>
            SteamUser.RequestStoreAuthURL(pchRedirectURL);

        /// <summary>
        /// <para> 获取用户的电话号码是否已验证</para>
        /// </summary>
        public static bool IsPhoneVerified() => SteamUser.BIsPhoneVerified();

        /// <summary>
        /// <para> 获取用户是否在其账户上启用了两步验证</para>
        /// </summary>
        public static bool IsTwoFactorEnabled() => SteamUser.BIsTwoFactorEnabled();

        /// <summary>
        /// <para> 获取用户的电话号码是否用于身份识别</para>
        /// </summary>
        public static bool IsPhoneIdentifying() => SteamUser.BIsPhoneIdentifying();

        /// <summary>
        /// <para> 获取用户的电话号码是否需要（重新）验证</para>
        /// </summary>
        public static bool IsPhoneRequiringVerification() => SteamUser.BIsPhoneRequiringVerification();

        /// <summary>
        /// <para> 获取市场资格</para>
        /// </summary>
        public static SteamAPICall_t GetMarketEligibility() => SteamUser.GetMarketEligibility();

        /// <summary>
        /// <para> 获取当前用户的防沉迷/时长控制</para>
        /// </summary>
        public static SteamAPICall_t GetDurationControl() => SteamUser.GetDurationControl();

        /// <summary>
        /// <para> 通知Steam中国时长控制系统游戏的在线状态。</para>
        /// <para> 这将防止离线游戏时间计入用户的游戏时间限制。</para>
        /// </summary>
        public static bool SetDurationControlOnlineState(EDurationControlOnlineState eNewState) =>
            SteamUser.BSetDurationControlOnlineState(eNewState);
    }
}