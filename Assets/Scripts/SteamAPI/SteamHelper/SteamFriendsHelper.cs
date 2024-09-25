using Steamworks;

namespace SteamAPI.SteamHelper
{
    public static class SteamFriendsHelper
    {
        /// <summary>
        /// 获取当前玩家的名称。
        /// 这是与用户社区个人资料页面上相同的名称。
        /// 该名称以UTF-8格式存储。
        /// 与所有其他返回char*的接口函数一样，重要的是不要保存此指针；它最终会被释放或重新分配。
        /// </summary>
        public static string GetPersonaName() => SteamFriends.GetPersonaName();

        /// <summary>
        /// 设置玩家名称，将其存储在服务器上，并向所有在线好友发布更改。
        /// 更改会立即在本地生效，并发布一个PersonaStateChange_t事件，假设成功。
        /// 最终结果通过返回值SteamAPICall_t获得，使用SetPersonaNameResponse_t。
        /// 如果名称更改在服务器上失败，则会额外发布一个全局PersonaStateChange_t事件来将名称改回，除了SetPersonaNameResponse_t回调之外。
        /// </summary>
        public static SteamAPICall_t SetPersonaName(string pchPersonaName) => SteamFriends.SetPersonaName(pchPersonaName);

        /// <summary>
        /// 获取当前用户的状态
        /// </summary>
        public static EPersonaState GetPersonaState() => SteamFriends.GetPersonaState();

        /// <summary>
        /// 获取符合指定好友标志的好友数量
        /// 接受一组k_EFriendFlags，并返回客户端知道的符合该条件的用户数量
        /// 然后可以使用GetFriendByIndex()返回这些用户中每个用户的ID
        /// </summary>
        public static int GetFriendCount(EFriendFlags iFriendFlags) => SteamFriends.GetFriendCount(iFriendFlags);

        /// <summary>
        /// 通过索引获取好友的SteamID
        /// iFriend是范围[0, GetFriendCount())内的索引
        /// iFriendsFlags必须与GetFriendCount()中使用的值相同
        /// 返回的CSteamID然后可以用于下面的所有函数来访问有关用户的详细信息
        /// </summary>
        public static CSteamID GetFriendByIndex(int iFriend, EFriendFlags iFriendFlags) =>
            SteamFriends.GetFriendByIndex(iFriend, iFriendFlags);

        /// <summary>
        /// 获取与指定用户的好友关系
        /// </summary>
        public static EFriendRelationship GetFriendRelationship(CSteamID steamIDFriend) =>
            SteamFriends.GetFriendRelationship(steamIDFriend);

        /// <summary>
        /// 获取指定好友的当前状态
        /// 只有当steamIDFriend在本地用户的好友列表中、在同一游戏服务器上、在聊天室或大厅中、或在与本地用户的小组中时，本地用户才会知道这些信息
        /// </summary>
        public static EPersonaState GetFriendPersonaState(CSteamID steamIDFriend) =>
            SteamFriends.GetFriendPersonaState(steamIDFriend);

        /// <summary>
        /// 获取指定好友的名称 - 保证不为NULL。
        /// 与GetFriendPersonaState()相同的规则适用于用户是否知道其他用户的名称
        /// 请注意，在首次加入大厅、聊天室或游戏服务器时，本地用户不会自动知道其他用户的名称；该信息将异步到达
        /// </summary>
        public static string GetFriendPersonaName(CSteamID steamIDFriend) =>
            SteamFriends.GetFriendPersonaName(steamIDFriend);

        /// <summary>
        /// 获取好友的游戏信息
        /// 如果好友确实在游戏中，则返回true，并用额外的详细信息填充pFriendGameInfo
        /// </summary>
        public static bool GetFriendGamePlayed(CSteamID steamIDFriend, out FriendGameInfo_t pFriendGameInfo) =>
            SteamFriends.GetFriendGamePlayed(steamIDFriend, out pFriendGameInfo);

        /// <summary>
        /// 获取好友的历史名称
        /// 返回历史记录中没有更多项目时返回一个空字符串
        /// </summary>
        public static string GetFriendPersonaNameHistory(CSteamID steamIDFriend, int iPersonaName) =>
            SteamFriends.GetFriendPersonaNameHistory(steamIDFriend, iPersonaName);

        /// <summary>
        /// 获取好友的Steam等级
        /// </summary>
        public static int GetFriendSteamLevel(CSteamID steamIDFriend) => SteamFriends.GetFriendSteamLevel(steamIDFriend);

        /// <summary>
        /// 获取玩家的昵称（已弃用）
        /// 返回当前用户为指定玩家设置的昵称。如果没有为该玩家设置昵称，则返回NULL。
        /// 已弃用：GetPersonaName遵循Steam昵称偏好，因此应用程序不应该明确关心昵称。
        /// </summary>
        public static string GetPlayerNickname(CSteamID steamIDPlayer) => SteamFriends.GetPlayerNickname(steamIDPlayer);

        /// <summary>
        /// 获取好友分组数量
        /// </summary>
        public static int GetFriendsGroupCount() => SteamFriends.GetFriendsGroupCount();

        /// <summary>
        /// 通过索引获取好友分组ID
        /// 返回给定索引的好友分组ID（无效索引返回k_FriendsGroupID_Invalid）
        /// </summary>
        public static FriendsGroupID_t GetFriendsGroupIDByIndex(int iFg) => SteamFriends.GetFriendsGroupIDByIndex(iFg);

        /// <summary>
        /// 获取好友分组名称
        /// 返回给定好友分组的名称（无效好友分组ID返回NULL）
        /// </summary>
        public static string GetFriendsGroupName(FriendsGroupID_t friendsGroupID) =>
            SteamFriends.GetFriendsGroupName(friendsGroupID);

        /// <summary>
        /// 获取指定好友分组的成员数量
        /// </summary>
        public static int GetFriendsGroupMembersCount(FriendsGroupID_t friendsGroupID) =>
            SteamFriends.GetFriendsGroupMembersCount(friendsGroupID);

        /// <summary>
        /// 获取指定好友分组的成员列表
        /// 获取给定好友分组的最多nMembersCount个成员，如果存在的成员少于请求的数量，那些位置的SteamID将无效
        /// </summary>
        public static void GetFriendsGroupMembersList(FriendsGroupID_t friendsGroupID, CSteamID[] pOutSteamIDMembers,
            int nMembersCount) =>
            SteamFriends.GetFriendsGroupMembersList(friendsGroupID, pOutSteamIDMembers, nMembersCount);

        /// <summary>
        /// 检查指定用户是否符合任一好友标志
        /// 如果指定用户符合iFriendFlags中指定的任何条件，则返回true
        /// iFriendFlags可以是一个或多个k_EFriendFlags值的并集（二进制或，|）
        /// </summary>
        public static bool HasFriend(CSteamID steamIDFriend, EFriendFlags iFriendFlags) =>
            SteamFriends.HasFriend(steamIDFriend, iFriendFlags);

        /// <summary>
        /// 获取公会（Clan）数量
        /// </summary>
        public static int GetClanCount() => SteamFriends.GetClanCount();

        /// <summary>
        /// 通过索引获取公会的SteamID
        /// </summary>
        public static CSteamID GetClanByIndex(int iClan) => SteamFriends.GetClanByIndex(iClan);

        /// <summary>
        /// 获取公会名称
        /// </summary>
        public static string GetClanName(CSteamID steamIDClan) => SteamFriends.GetClanName(steamIDClan);

        /// <summary>
        /// 获取公会标签
        /// </summary>
        public static string GetClanTag(CSteamID steamIDClan) => SteamFriends.GetClanTag(steamIDClan);

        /// <summary>
        /// 获取公会的活动计数
        /// 返回我们所拥有的关于公会中正在发生的事情的最新信息
        /// </summary>
        public static bool GetClanActivityCounts(CSteamID steamIDClan, out int pnOnline, out int pnInGame,
            out int pnChatting) =>
            SteamFriends.GetClanActivityCounts(steamIDClan, out pnOnline, out pnInGame, out pnChatting);

        /// <summary>
        /// 下载公会活动计数
        /// 对于用户所属的公会，他们将拥有相当最新的信息，但对于其他公会，你需要下载信息以获得最新数据
        /// </summary>
        public static SteamAPICall_t DownloadClanActivityCounts(CSteamID[] psteamIDClans, int cClansToRequest) =>
            SteamFriends.DownloadClanActivityCounts(psteamIDClans, cClansToRequest);

        /// <summary>
        /// 获取指定来源的好友数量
        /// 用于获取聊天室、大厅、游戏服务器或公会中的用户迭代器
        /// 请注意，大型公会无法被本地用户迭代
        /// 请注意，当前用户必须在大厅中才能检索该大厅中其他用户的CSteamID
        /// steamIDSource可以是群组、游戏服务器、大厅或聊天室的steamID
        /// </summary>
        public static int GetFriendCountFromSource(CSteamID steamIDSource) =>
            SteamFriends.GetFriendCountFromSource(steamIDSource);

        /// <summary>
        /// 通过索引获取指定来源的好友SteamID
        /// </summary>
        public static CSteamID GetFriendFromSourceByIndex(CSteamID steamIDSource, int iFriend) =>
            SteamFriends.GetFriendFromSourceByIndex(steamIDSource, iFriend);

        /// <summary>
        /// 检查用户是否在指定来源中
        /// 如果本地用户可以看到steamIDUser是steamIDSource的成员或在其中，则返回true
        /// </summary>
        public static bool IsUserInSource(CSteamID steamIDUser, CSteamID steamIDSource) =>
            SteamFriends.IsUserInSource(steamIDUser, steamIDSource);

        /// <summary>
        /// 设置游戏中的语音状态
        /// 用户正在游戏中按下说话按钮（将抑制Steam好友UI中所有语音通信的麦克风）
        /// </summary>
        public static void SetInGameVoiceSpeaking(CSteamID steamIDUser, bool bSpeaking) =>
            SteamFriends.SetInGameVoiceSpeaking(steamIDUser, bSpeaking);

        /// <summary>
        /// 激活游戏覆盖层，并可选择打开特定对话框
        /// 激活游戏覆盖层，可选择打开一个特定的对话框
        /// 有效选项包括"Friends"、"Community"、"Players"、"Settings"、"OfficialGameGroup"、"Stats"、"Achievements"、
        /// "chatroomgroup/nnnn"
        /// </summary>
        public static void ActivateGameOverlay(string pchDialog) => SteamFriends.ActivateGameOverlay(pchDialog);

        /// <summary>
        /// 激活游戏覆盖层到特定用户
        /// 激活游戏覆盖层到特定位置
        /// 有效选项是：
        ///     "steamid" - 打开覆盖层网页浏览器到指定用户或群组的个人资料
        ///     "chat" - 打开与指定用户的聊天窗口，或加入群组聊天
        ///     "jointrade" - 打开一个窗口到使用ISteamEconomy/StartTrade Web API启动的Steam交易会话
        ///     "stats" - 打开覆盖层网页浏览器到指定用户的统计信息
        ///     "achievements" - 打开覆盖层网页浏览器到指定用户的成就
        ///     "friendadd" - 以最小模式打开覆盖层，提示用户将目标用户添加为好友
        ///     "friendremove" - 以最小模式打开覆盖层，提示用户删除目标好友
        ///     "friendrequestaccept" - 以最小模式打开覆盖层，提示用户接受传入的好友邀请
        ///     "friendrequestignore" - 以最小模式打开覆盖层，提示用户忽略传入的好友邀请
        /// </summary>
        public static void ActivateGameOverlayToUser(string pchDialog, CSteamID steamID) =>
            SteamFriends.ActivateGameOverlayToUser(pchDialog, steamID);

        /// <summary>
        /// 激活游戏覆盖层到指定网页
        /// 直接激活游戏覆盖层网页浏览器到指定的URL
        /// 需要完整的地址和协议类型，例如 http://www.steamgames.com/
        /// </summary>
        public static void ActivateGameOverlayToWebPage(string pchURL,
            EActivateGameOverlayToWebPageMode eMode =
                EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default) =>
            SteamFriends.ActivateGameOverlayToWebPage(pchURL, eMode);

        /// <summary>
        /// 激活游戏覆盖层到应用商店页面
        /// 激活游戏覆盖层到应用的商店页面
        /// </summary>
        public static void ActivateGameOverlayToStore(AppId_t nAppID, EOverlayToStoreFlag eFlag) =>
            SteamFriends.ActivateGameOverlayToStore(nAppID, eFlag);

        /// <summary>
        /// 标记目标用户为"已互动"
        /// 将目标用户标记为"已玩过"。这是一个仅客户端的功能，要求调用用户在游戏中
        /// </summary>
        public static void SetPlayedWith(CSteamID steamIDUserPlayedWith) =>
            SteamFriends.SetPlayedWith(steamIDUserPlayedWith);

        /// <summary>
        /// 激活游戏覆盖层邀请对话框
        /// 激活游戏覆盖层以打开邀请对话框。将为提供的大厅发送邀请。
        /// </summary>
        public static void ActivateGameOverlayInviteDialog(CSteamID steamIDLobby) =>
            SteamFriends.ActivateGameOverlayInviteDialog(steamIDLobby);

        /// <summary>
        /// 获取好友的小头像（32x32）
        /// 获取当前用户的小头像（32x32），这是一个用于IClientUtils::GetImageRGBA()的句柄，如果未设置则为0
        /// </summary>
        public static int GetSmallFriendAvatar(CSteamID steamIDFriend) => SteamFriends.GetSmallFriendAvatar(steamIDFriend);

        /// <summary>
        /// 获取好友的中头像（64x64）
        /// 获取当前用户的中头像（64x64），这是一个用于IClientUtils::GetImageRGBA()的句柄，如果未设置则为0
        /// </summary>
        public static int GetMediumFriendAvatar(CSteamID steamIDFriend) =>
            SteamFriends.GetMediumFriendAvatar(steamIDFriend);

        /// <summary>
        /// 获取好友的大头像（184x184）
        /// 获取当前用户的大头像（184x184），这是一个用于IClientUtils::GetImageRGBA()的句柄，如果未设置则为0
        /// 如果这个图像尚未加载，则返回-1，在这种情况下等待AvatarImageLoaded_t回调，然后再次调用此函数
        /// </summary>
        public static int GetLargeFriendAvatar(CSteamID steamIDFriend) => SteamFriends.GetLargeFriendAvatar(steamIDFriend);

        /// <summary>
        /// 请求指定用户的信息（昵称和头像）
        /// 请求有关用户的信息 - 个性化名称和头像
        /// 如果设置了bRequireNameOnly，则不会下载用户的头像
        /// - 下载头像要慢得多，并且会搅动本地缓存，所以如果不需要头像，就不要请求它们
        /// 如果返回true，这意味着数据正在被请求，当检索到数据时将发布PersonaStateChanged_t回调
        /// 如果返回false，这意味着我们已经有了关于该用户的所有详细信息，可以立即调用函数
        /// </summary>
        public static bool RequestUserInformation(CSteamID steamIDUser, bool bRequireNameOnly) =>
            SteamFriends.RequestUserInformation(steamIDUser, bRequireNameOnly);

        /// <summary>
        /// 请求公会官员列表
        /// 请求公会官员列表
        /// 完成后，数据在ClanOfficerListResponse_t调用结果中返回
        /// 这使得下面的调用可用
        /// 你只能询问用户所属的公会
        /// 请注意，这不会自动下载头像；如果你得到一个官员，
        /// 并且没有可用的头像图像，调用RequestUserInformation( steamID, false )来下载头像
        /// </summary>
        public static SteamAPICall_t RequestClanOfficerList(CSteamID steamIDClan) =>
            SteamFriends.RequestClanOfficerList(steamIDClan);

        /// <summary>
        /// 获取公会所有者的SteamID
        /// 公会官员迭代 - 只能在RequestClanOfficerList()调用完成时进行
        /// 返回公会所有者的steamID
        /// </summary>
        public static CSteamID GetClanOwner(CSteamID steamIDClan) => SteamFriends.GetClanOwner(steamIDClan);

        /// <summary>
        /// 获取公会官员数量
        /// 返回公会中的官员数量（包括所有者）
        /// </summary>
        public static int GetClanOfficerCount(CSteamID steamIDClan) => SteamFriends.GetClanOfficerCount(steamIDClan);

        /// <summary>
        /// 通过索引获取公会官员的SteamID
        /// 返回公会官员的steamID，按索引，范围[0,GetClanOfficerCount)
        /// </summary>
        public static CSteamID GetClanOfficerByIndex(CSteamID steamIDClan, int iOfficer) =>
            SteamFriends.GetClanOfficerByIndex(steamIDClan, iOfficer);

        /// <summary>
        /// 检查当前用户是否被聊天限制
        /// 如果当前用户被聊天限制，他就不能发送或接收任何文本/语音聊天消息。
        /// 用户看不到自定义头像。但用户可以在线并发送/接收游戏邀请。
        /// 被聊天限制的用户不能添加好友或加入任何群组。
        /// </summary>
        public static uint GetUserRestrictions() => SteamFriends.GetUserRestrictions();

        /// <summary>
        /// 设置富文本存在
        /// 富文本存在数据在同一游戏中的朋友之间自动共享
        /// 每个用户都有一组键/值对
        /// 注意以下限制：k_cchMaxRichPresenceKeys, k_cchMaxRichPresenceKeyLength, k_cchMaxRichPresenceValueLength
        /// 有五个魔法键：
        ///     "status"  - 一个UTF-8字符串，将显示在Steam好友列表中的"查看游戏信息"对话框中
        ///     "connect" - 一个UTF-8字符串，包含朋友如何连接到游戏的命令行
        ///     "steam_display"              - 命名一个富文本存在本地化令牌，将以查看用户选择的语言在Steam客户端UI中显示。
        ///                                    更多信息：https://partner.steamgames.com/doc/api/ISteamFriends#richpresencelocalization
        ///     "steam_player_group"         - 当设置时，向Steam客户端表明玩家是特定组的成员。同一组的玩家
        ///                                    可能会在Steam UI的各个地方被组织在一起。
        ///     "steam_player_group_size"    - 当设置时，表示steam_player_group中的玩家总数。当组的所有成员
        ///                                    不是用户好友列表的一部分时，Steam客户端可能会使用这个数字来显示关于组的额外信息。
        /// GetFriendRichPresence() 如果没有设置值则返回一个空字符串 ""
        /// SetRichPresence() 设置为NULL或空字符串将删除该键
        /// 你可以使用GetFriendRichPresenceKeyCount()和GetFriendRichPresenceKeyByIndex()迭代当前键的集合
        /// （通常仅用于调试）
        /// </summary>
        public static bool SetRichPresence(string pchKey, string pchValue) =>
            SteamFriends.SetRichPresence(pchKey, pchValue);

        /// <summary>
        /// 清除所有富文本存在
        /// </summary>
        public static void ClearRichPresence() => SteamFriends.ClearRichPresence();

        /// <summary>
        /// 获取好友的富文本存在
        /// </summary>
        public static string GetFriendRichPresence(CSteamID steamIDFriend, string pchKey) =>
            SteamFriends.GetFriendRichPresence(steamIDFriend, pchKey);

        /// <summary>
        /// 获取好友的富文本存在键数量
        /// </summary>
        public static int GetFriendRichPresenceKeyCount(CSteamID steamIDFriend) =>
            SteamFriends.GetFriendRichPresenceKeyCount(steamIDFriend);

        /// <summary>
        /// 通过索引获取好友的富文本存在键
        /// </summary>
        public static string GetFriendRichPresenceKeyByIndex(CSteamID steamIDFriend, int iKey) =>
            SteamFriends.GetFriendRichPresenceKeyByIndex(steamIDFriend, iKey);

        /// <summary>
        /// 请求指定好友的富文本存在
        /// 请求特定用户的富文本存在。
        /// </summary>
        public static void RequestFriendRichPresence(CSteamID steamIDFriend) =>
            SteamFriends.RequestFriendRichPresence(steamIDFriend);

        /// <summary>
        /// 发送富邀请
        /// 富邀请支持。
        /// 如果目标接受邀请，将发布一个包含连接字符串的GameRichPresenceJoinRequested_t回调。
        /// （或者你可以配置你的游戏，使其在命令行上传递。这是一个已弃用的路径；如果你真的需要这个，请询问我们。）
        /// </summary>
        public static bool InviteUserToGame(CSteamID steamIDFriend, string pchConnectString) =>
            SteamFriends.InviteUserToGame(steamIDFriend, pchConnectString);

        /// <summary>
        /// 获取最近一起玩的好友数量
        /// 最近一起玩的好友迭代
        /// 这会迭代整个最近一起玩的用户列表，跨游戏
        /// GetFriendCoplayTime()返回Unix时间
        /// </summary>
        public static int GetCoplayFriendCount() => SteamFriends.GetCoplayFriendCount();

        /// <summary>
        /// 通过索引获取最近一起玩的好友SteamID
        /// </summary>
        public static CSteamID GetCoplayFriend(int iCoplayFriend) => SteamFriends.GetCoplayFriend(iCoplayFriend);

        /// <summary>
        /// 获取好友一起玩的时间（Unix时间）
        /// </summary>
        public static int GetFriendCoplayTime(CSteamID steamIDFriend) => SteamFriends.GetFriendCoplayTime(steamIDFriend);

        /// <summary>
        /// 获取好友一起玩的游戏AppID
        /// </summary>
        public static AppId_t GetFriendCoplayGame(CSteamID steamIDFriend) =>
            SteamFriends.GetFriendCoplayGame(steamIDFriend);

        /// <summary>
        /// 加入公会聊天房间
        /// 游戏的聊天界面
        /// 这允许从游戏内访问群组（公会）聊天
        /// 行为相当复杂，因为用户可能已经在游戏外或覆盖层中的群组聊天中
        /// 使用ActivateGameOverlayToUser( "chat", steamIDClan )打开游戏内覆盖层版本的聊天
        /// </summary>
        public static SteamAPICall_t JoinClanChatRoom(CSteamID steamIDClan) => SteamFriends.JoinClanChatRoom(steamIDClan);

        /// <summary>
        /// 离开公会聊天房间
        /// </summary>
        public static bool LeaveClanChatRoom(CSteamID steamIDClan) => SteamFriends.LeaveClanChatRoom(steamIDClan);

        /// <summary>
        /// 获取公会聊天消息数量
        /// </summary>
        public static int GetClanChatMemberCount(CSteamID steamIDClan) => SteamFriends.GetClanChatMemberCount(steamIDClan);

        /// <summary>
        /// 通过索引获取公会聊天成员的SteamID
        /// </summary>
        public static CSteamID GetChatMemberByIndex(CSteamID steamIDClan, int iUser) =>
            SteamFriends.GetChatMemberByIndex(steamIDClan, iUser);

        /// <summary>
        /// 发送公会聊天消息
        /// </summary>
        public static bool SendClanChatMessage(CSteamID steamIDClanChat, string pchText) =>
            SteamFriends.SendClanChatMessage(steamIDClanChat, pchText);

        /// <summary>
        /// 获取公会聊天消息
        /// </summary>
        public static int GetClanChatMessage(CSteamID steamIDClanChat, int iMessage, out string prgchText, int cchTextMax,
            out EChatEntryType peChatEntryType, out CSteamID psteamidChatter) => SteamFriends.GetClanChatMessage(
            steamIDClanChat, iMessage, out prgchText, cchTextMax, out peChatEntryType, out psteamidChatter);

        /// <summary>
        /// 检查用户是否为公会聊天管理员
        /// </summary>
        public static bool IsClanChatAdmin(CSteamID steamIDClanChat, CSteamID steamIDUser) =>
            SteamFriends.IsClanChatAdmin(steamIDClanChat, steamIDUser);

        /// <summary>
        /// 检查公会聊天窗口是否在Steam中打开
        /// </summary>
        public static bool IsClanChatWindowOpenInSteam(CSteamID steamIDClanChat) =>
            SteamFriends.IsClanChatWindowOpenInSteam(steamIDClanChat);

        /// <summary>
        /// 在Steam中打开公会聊天窗口
        /// </summary>
        public static bool OpenClanChatWindowInSteam(CSteamID steamIDClanChat) =>
            SteamFriends.OpenClanChatWindowInSteam(steamIDClanChat);

        /// <summary>
        /// 在Steam中关闭公会聊天窗口
        /// </summary>
        public static bool CloseClanChatWindowInSteam(CSteamID steamIDClanChat) =>
            SteamFriends.CloseClanChatWindowInSteam(steamIDClanChat);

        /// <summary>
        /// 设置是否监听好友消息
        /// </summary>
        public static bool SetListenForFriendsMessages(bool bInterceptEnabled) =>
            SteamFriends.SetListenForFriendsMessages(bInterceptEnabled);

        /// <summary>
        /// 回复好友消息
        /// </summary>
        public static bool ReplyToFriendMessage(CSteamID steamIDFriend, string pchMsgToSend) =>
            SteamFriends.ReplyToFriendMessage(steamIDFriend, pchMsgToSend);

        /// <summary>
        /// 获取好友消息
        /// </summary>
        public static int GetFriendMessage(CSteamID steamIDFriend, int iMessageID, out string pvData, int cubData,
            out EChatEntryType peChatEntryType) =>
            SteamFriends.GetFriendMessage(steamIDFriend, iMessageID, out pvData, cubData, out peChatEntryType);

        /// <summary>
        /// 获取关注者数量
        /// </summary>
        public static SteamAPICall_t GetFollowerCount(CSteamID steamID) => SteamFriends.GetFollowerCount(steamID);

        /// <summary>
        /// 检查是否正在关注用户
        /// </summary>
        public static SteamAPICall_t IsFollowing(CSteamID steamID) => SteamFriends.IsFollowing(steamID);

        /// <summary>
        /// 枚举关注列表
        /// </summary>
        public static SteamAPICall_t EnumerateFollowingList(uint unStartIndex) =>
            SteamFriends.EnumerateFollowingList(unStartIndex);

        /// <summary>
        /// 检查公会是否为公共公会
        /// </summary>
        public static bool IsClanPublic(CSteamID steamIDClan) => SteamFriends.IsClanPublic(steamIDClan);

        /// <summary>
        /// 检查公会是否为官方游戏组
        /// </summary>
        public static bool IsClanOfficialGameGroup(CSteamID steamIDClan) =>
            SteamFriends.IsClanOfficialGameGroup(steamIDClan);

        /// <summary>
        /// 获取有未读优先级消息的聊天数量
        /// </summary>
        public static int GetNumChatsWithUnreadPriorityMessages() => SteamFriends.GetNumChatsWithUnreadPriorityMessages();

        /// <summary>
        /// 激活游戏覆盖层远程播放邀请对话框
        /// </summary>
        public static void ActivateGameOverlayRemotePlayTogetherInviteDialog(CSteamID steamIDLobby) =>
            SteamFriends.ActivateGameOverlayRemotePlayTogetherInviteDialog(steamIDLobby);

        /// <summary>
        /// 注册协议到覆盖浏览器
        /// </summary>
        public static bool RegisterProtocolInOverlayBrowser(string pchProtocol) =>
            SteamFriends.RegisterProtocolInOverlayBrowser(pchProtocol);

        /// <summary>
        /// 激活游戏覆盖层邀请对话框并连接字符串
        /// </summary>
        public static void ActivateGameOverlayInviteDialogConnectString(string pchConnectString) =>
            SteamFriends.ActivateGameOverlayInviteDialogConnectString(pchConnectString);

        /// <summary>
        /// 请求装备的个人资料物品
        /// </summary>
        public static SteamAPICall_t RequestEquippedProfileItems(CSteamID steamID) =>
            SteamFriends.RequestEquippedProfileItems(steamID);

        /// <summary>
        /// 检查用户是否装备了指定类型的个人资料物品
        /// </summary>
        public static bool BHasEquippedProfileItem(CSteamID steamID, ECommunityProfileItemType itemType) =>
            SteamFriends.BHasEquippedProfileItem(steamID, itemType);

        /// <summary>
        /// 获取装备的个人资料物品属性（字符串）
        /// </summary>
        public static string GetProfileItemPropertyString(CSteamID steamID, ECommunityProfileItemType itemType,
            ECommunityProfileItemProperty prop) => SteamFriends.GetProfileItemPropertyString(steamID, itemType, prop);

        /// <summary>
        /// 获取装备的个人资料物品属性（无符号整数）
        /// </summary>
        public static uint GetProfileItemPropertyUint(CSteamID steamID, ECommunityProfileItemType itemType,
            ECommunityProfileItemProperty prop) => SteamFriends.GetProfileItemPropertyUint(steamID, itemType, prop);
    }
}