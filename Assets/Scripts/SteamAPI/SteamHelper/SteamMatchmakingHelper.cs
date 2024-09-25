using Steamworks;

namespace SteamAPI.SteamHelper
{
    public static class SteamMatchmakingHelper
    {
        /// <summary>
        /// 获取收藏游戏的数量
        /// </summary>
        /// <returns>返回收藏游戏的数量</returns>
        public static int GetFavoriteGameCount() => SteamMatchmaking.GetFavoriteGameCount();

        /// <summary>
        /// 获取收藏游戏的详细信息
        /// </summary>
        /// <param name="index">游戏索引</param>
        /// <param name="appId">应用ID</param>
        /// <param name="ip">IP地址</param>
        /// <param name="connPort">连接端口</param>
        /// <param name="queryPort">查询端口</param>
        /// <param name="flags">标志</param>
        /// <param name="lastPlayedOnServer">最后一次在服务器上玩的时间</param>
        /// <returns>是否成功获取信息</returns>
        public static bool GetFavoriteGame(int index, out AppId_t appId, out uint ip, out ushort connPort,
            out ushort queryPort, out uint flags, out uint lastPlayedOnServer) =>
            SteamMatchmaking.GetFavoriteGame(index, out appId, out ip, out connPort, out queryPort, out flags,
                out lastPlayedOnServer);

        /// <summary>
        /// 添加收藏游戏
        /// </summary>
        /// <param name="appId">应用ID</param>
        /// <param name="ip">IP地址</param>
        /// <param name="connPort">连接端口</param>
        /// <param name="queryPort">查询端口</param>
        /// <param name="flags">标志</param>
        /// <param name="rTime32LastPlayedOnServer">最后一次在服务器上玩的时间</param>
        /// <returns>是否成功添加</returns>
        public static int AddFavoriteGame(AppId_t appId, uint ip, ushort connPort, ushort queryPort, uint flags,
            uint rTime32LastPlayedOnServer) =>
            SteamMatchmaking.AddFavoriteGame(appId, ip, connPort, queryPort, flags, rTime32LastPlayedOnServer);

        /// <summary>
        /// 移除收藏游戏
        /// </summary>
        /// <param name="appId">应用ID</param>
        /// <param name="ip">IP地址</param>
        /// <param name="connPort">连接端口</param>
        /// <param name="queryPort">查询端口</param>
        /// <param name="flags">标志</param>
        /// <returns>是否成功移除</returns>
        public static bool RemoveFavoriteGame(AppId_t appId, uint ip, ushort connPort, ushort queryPort, uint flags) =>
            SteamMatchmaking.RemoveFavoriteGame(appId, ip, connPort, queryPort, flags);

        /// <summary>
        /// 请求大厅列表
        /// </summary>
        /// <returns>Steam API 调用结果</returns>
        public static SteamAPICall_t RequestLobbyList() =>
            SteamMatchmaking.RequestLobbyList();

        /// <summary>
        /// 添加大厅列表字符串过滤器
        /// </summary>
        /// <param name="pchKeyToMatch">匹配的键</param>
        /// <param name="pchValueToMatch">匹配的值</param>
        /// <param name="eComparisonType">比较类型</param>
        public static void AddRequestLobbyListStringFilter(string pchKeyToMatch, string pchValueToMatch,
            ELobbyComparison eComparisonType) =>
            SteamMatchmaking.AddRequestLobbyListStringFilter(pchKeyToMatch, pchValueToMatch, eComparisonType);

        /// <summary>
        /// 添加大厅列表数值过滤器
        /// </summary>
        /// <param name="pchKeyToMatch">匹配的键</param>
        /// <param name="nValueToMatch">匹配的值</param>
        /// <param name="eComparisonType">比较类型</param>
        public static void AddRequestLobbyListNumericalFilter(string pchKeyToMatch, int nValueToMatch,
            ELobbyComparison eComparisonType) =>
            SteamMatchmaking.AddRequestLobbyListNumericalFilter(pchKeyToMatch, nValueToMatch, eComparisonType);

        /// <summary>
        /// 添加大厅列表近值过滤器
        /// </summary>
        /// <param name="pchKeyToMatch">匹配的键</param>
        /// <param name="nValueToBeCloseTo">接近的值</param>
        public static void AddRequestLobbyListNearValueFilter(string pchKeyToMatch, int nValueToBeCloseTo) =>
            SteamMatchmaking.AddRequestLobbyListNearValueFilter(pchKeyToMatch, nValueToBeCloseTo);

        /// <summary>
        /// 添加大厅列表可用插槽过滤器
        /// </summary>
        /// <param name="nSlotsAvailable">可用插槽数量</param>
        public static void AddRequestLobbyListFilterSlotsAvailable(int nSlotsAvailable) =>
            SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(nSlotsAvailable);

        /// <summary>
        /// 添加大厅列表距离过滤器
        /// </summary>
        /// <param name="eLobbyDistanceFilter">距离过滤器类型</param>
        public static void AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter eLobbyDistanceFilter) =>
            SteamMatchmaking.AddRequestLobbyListDistanceFilter(eLobbyDistanceFilter);

        /// <summary>
        /// 添加大厅列表结果数量过滤器
        /// </summary>
        /// <param name="cMaxResults">最大结果数量</param>
        public static void AddRequestLobbyListResultCountFilter(int cMaxResults) =>
            SteamMatchmaking.AddRequestLobbyListResultCountFilter(cMaxResults);

        /// <summary>
        /// 添加大厅列表兼容成员过滤器
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        public static void AddRequestLobbyListCompatibleMembersFilter(CSteamID steamIDLobby) =>
            SteamMatchmaking.AddRequestLobbyListCompatibleMembersFilter(steamIDLobby);

        /// <summary>
        /// 根据索引获取大厅
        /// </summary>
        /// <param name="iLobby">大厅索引</param>
        /// <returns>大厅的CSteamID</returns>
        public static CSteamID GetLobbyByIndex(int iLobby) =>
            SteamMatchmaking.GetLobbyByIndex(iLobby);

        /// <summary>
        /// 创建大厅
        /// </summary>
        /// <param name="eLobbyType">大厅类型</param>
        /// <param name="cMaxMembers">最大成员数量</param>
        /// <returns>Steam API 调用结果</returns>
        public static SteamAPICall_t CreateLobby(ELobbyType eLobbyType, int cMaxMembers) =>
            SteamMatchmaking.CreateLobby(eLobbyType, cMaxMembers);

        /// <summary>
        /// 加入大厅
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <returns>Steam API 调用结果</returns>
        public static SteamAPICall_t JoinLobby(CSteamID steamIDLobby) =>
            SteamMatchmaking.JoinLobby(steamIDLobby);

        /// <summary>
        /// 离开大厅
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        public static void LeaveLobby(CSteamID steamIDLobby) =>
            SteamMatchmaking.LeaveLobby(steamIDLobby);

        /// <summary>
        /// 邀请用户加入大厅
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="steamIDInvitee">被邀请用户的ID</param>
        /// <returns>是否成功发送邀请</returns>
        public static bool InviteUserToLobby(CSteamID steamIDLobby, CSteamID steamIDInvitee) =>
            SteamMatchmaking.InviteUserToLobby(steamIDLobby, steamIDInvitee);

        /// <summary>
        /// 获取大厅成员数量
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <returns>大厅成员数量</returns>
        public static int GetNumLobbyMembers(CSteamID steamIDLobby) =>
            SteamMatchmaking.GetNumLobbyMembers(steamIDLobby);

        /// <summary>
        /// 根据索引获取大厅成员
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="iMember">成员索引</param>
        /// <returns>成员的CSteamID</returns>
        public static CSteamID GetLobbyMemberByIndex(CSteamID steamIDLobby, int iMember) =>
            SteamMatchmaking.GetLobbyMemberByIndex(steamIDLobby, iMember);

        /// <summary>
        /// 获取大厅数据
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="pchKey">键</param>
        /// <returns>对应的值</returns>
        public static string GetLobbyData(CSteamID steamIDLobby, string pchKey) =>
            SteamMatchmaking.GetLobbyData(steamIDLobby, pchKey);

        /// <summary>
        /// 设置大厅数据
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="pchKey">键</param>
        /// <param name="pchValue">值</param>
        /// <returns>是否成功设置</returns>
        public static bool SetLobbyData(CSteamID steamIDLobby, string pchKey, string pchValue) =>
            SteamMatchmaking.SetLobbyData(steamIDLobby, pchKey, pchValue);

        /// <summary>
        /// 获取大厅数据数量
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <returns>大厅数据数量</returns>
        public static int GetLobbyDataCount(CSteamID steamIDLobby) =>
            SteamMatchmaking.GetLobbyDataCount(steamIDLobby);

        /// <summary>
        /// 根据索引获取大厅数据
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="iLobbyData">数据索引</param>
        /// <param name="pchKey">键</param>
        /// <param name="cchKeyBufferSize">键缓冲区大小</param>
        /// <param name="pchValue">值</param>
        /// <param name="cchValueBufferSize">值缓冲区大小</param>
        /// <returns>是否成功获取</returns>
        public static bool GetLobbyDataByIndex(CSteamID steamIDLobby, int iLobbyData, out string pchKey,
            int cchKeyBufferSize, out string pchValue, int cchValueBufferSize) =>
            SteamMatchmaking.GetLobbyDataByIndex(steamIDLobby, iLobbyData, out pchKey, cchKeyBufferSize,
                out pchValue, cchValueBufferSize);

        /// <summary>
        /// 删除大厅数据
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="pchKey">键</param>
        /// <returns>是否成功删除</returns>
        public static bool DeleteLobbyData(CSteamID steamIDLobby, string pchKey) =>
            SteamMatchmaking.DeleteLobbyData(steamIDLobby, pchKey);

        /// <summary>
        /// 获取大厅成员数据
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="steamIDUser">用户ID</param>
        /// <param name="pchKey">键</param>
        /// <returns>对应的值</returns>
        public static string GetLobbyMemberData(CSteamID steamIDLobby, CSteamID steamIDUser, string pchKey) =>
            SteamMatchmaking.GetLobbyMemberData(steamIDLobby, steamIDUser, pchKey);

        /// <summary>
        /// 设置大厅成员数据
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="pchKey">键</param>
        /// <param name="pchValue">值</param>
        public static void SetLobbyMemberData(CSteamID steamIDLobby, string pchKey, string pchValue) =>
            SteamMatchmaking.SetLobbyMemberData(steamIDLobby, pchKey, pchValue);

        /// <summary>
        /// 发送大厅聊天消息
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="pvMsgBody">消息体</param>
        /// <param name="cubMsgBody">消息体大小</param>
        /// <returns>是否成功发送</returns>
        public static bool SendLobbyChatMsg(CSteamID steamIDLobby, byte[] pvMsgBody, int cubMsgBody) =>
            SteamMatchmaking.SendLobbyChatMsg(steamIDLobby, pvMsgBody, cubMsgBody);

        /// <summary>
        /// 获取大厅聊天条目
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="iChatID">聊天ID</param>
        /// <param name="pSteamIDUser">用户ID</param>
        /// <param name="pvData">数据</param>
        /// <param name="cubData">数据大小</param>
        /// <param name="peChatEntryType">聊天条目类型</param>
        /// <returns>写入缓冲区的字节数</returns>
        public static int GetLobbyChatEntry(CSteamID steamIDLobby, int iChatID, out CSteamID pSteamIDUser, byte[] pvData,
            int cubData, out EChatEntryType peChatEntryType) =>
            SteamMatchmaking.GetLobbyChatEntry(steamIDLobby, iChatID, out pSteamIDUser, pvData, cubData,
                out peChatEntryType);

        /// <summary>
        /// 请求大厅数据
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <returns>是否成功请求</returns>
        public static bool RequestLobbyData(CSteamID steamIDLobby) =>
            SteamMatchmaking.RequestLobbyData(steamIDLobby);

        /// <summary>
        /// 设置大厅游戏服务器
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="unGameServerIP">游戏服务器IP</param>
        /// <param name="unGameServerPort">游戏服务器端口</param>
        /// <param name="steamIDGameServer">游戏服务器ID</param>
        public static void SetLobbyGameServer(CSteamID steamIDLobby, uint unGameServerIP, ushort unGameServerPort,
            CSteamID steamIDGameServer) =>
            SteamMatchmaking.SetLobbyGameServer(steamIDLobby, unGameServerIP, unGameServerPort, steamIDGameServer);

        /// <summary>
        /// 获取大厅游戏服务器
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="punGameServerIP">游戏服务器IP</param>
        /// <param name="punGameServerPort">游戏服务器端口</param>
        /// <param name="psteamIDGameServer">游戏服务器ID</param>
        /// <returns>是否成功获取</returns>
        public static bool GetLobbyGameServer(CSteamID steamIDLobby, out uint punGameServerIP, out ushort punGameServerPort,
            out CSteamID psteamIDGameServer) =>
            SteamMatchmaking.GetLobbyGameServer(steamIDLobby, out punGameServerIP, out punGameServerPort,
                out psteamIDGameServer);

        /// <summary>
        /// 设置大厅成员限制
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="cMaxMembers">最大成员数量</param>
        /// <returns>是否成功设置</returns>
        public static bool SetLobbyMemberLimit(CSteamID steamIDLobby, int cMaxMembers) =>
            SteamMatchmaking.SetLobbyMemberLimit(steamIDLobby, cMaxMembers);

        /// <summary>
        /// 获取大厅成员限制
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <returns>最大成员数量</returns>
        public static int GetLobbyMemberLimit(CSteamID steamIDLobby) =>
            SteamMatchmaking.GetLobbyMemberLimit(steamIDLobby);

        /// <summary>
        /// 设置大厅类型
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="eLobbyType">大厅类型</param>
        /// <returns>是否成功设置</returns>
        public static bool SetLobbyType(CSteamID steamIDLobby, ELobbyType eLobbyType) =>
            SteamMatchmaking.SetLobbyType(steamIDLobby, eLobbyType);

        /// <summary>
        /// 设置大厅是否可加入
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="bLobbyJoinable">是否可加入</param>
        /// <returns>是否成功设置</returns>
        public static bool SetLobbyJoinable(CSteamID steamIDLobby, bool bLobbyJoinable) =>
            SteamMatchmaking.SetLobbyJoinable(steamIDLobby, bLobbyJoinable);

        /// <summary>
        /// 获取大厅所有者
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <returns>大厅所有者的CSteamID</returns>
        public static CSteamID GetLobbyOwner(CSteamID steamIDLobby) =>
            SteamMatchmaking.GetLobbyOwner(steamIDLobby);

        /// <summary>
        /// 设置大厅所有者
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="steamIDNewOwner">新所有者的CSteamID</param>
        /// <returns>是否成功设置</returns>
        public static bool SetLobbyOwner(CSteamID steamIDLobby, CSteamID steamIDNewOwner) =>
            SteamMatchmaking.SetLobbyOwner(steamIDLobby, steamIDNewOwner);

        /// <summary>
        /// 设置关联大厅
        /// </summary>
        /// <param name="steamIDLobby">大厅ID</param>
        /// <param name="steamIDLobbyDependent">关联大厅ID</param>
        /// <returns>是否成功设置</returns>
        public static bool SetLinkedLobby(CSteamID steamIDLobby, CSteamID steamIDLobbyDependent) =>
            SteamMatchmaking.SetLinkedLobby(steamIDLobby, steamIDLobbyDependent);
    }
}