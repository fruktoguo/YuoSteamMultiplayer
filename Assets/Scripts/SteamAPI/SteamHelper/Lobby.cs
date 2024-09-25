using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Steamworks;

namespace SteamAPI.SteamHelper
{
    public struct Lobby
    {
        public CSteamID Id { get; internal set; }

        public Lobby(CSteamID id) => Id = id;

        public static implicit operator Lobby(CSteamID steamId) => new() { Id = steamId };

        public static implicit operator CSteamID(Lobby lobby) => lobby.Id;

        public const string PasswordKey = "password"; // 密码键常量
        public const string NameKey = "name"; // 名称键常量

        public override string ToString()
        {
            return Id.ToString();
        }

        /// <summary>
        /// 获取房间名称
        /// </summary>
        /// <returns>返回房间名称</returns>
        public string LobbyName => SteamMatchmakingHelper.GetLobbyData(Id, NameKey);

        /// <summary>
        /// 获取房间当前人数
        /// </summary>
        /// <returns>返回当前房间人数</returns>
        public int MemberCount => SteamMatchmakingHelper.GetNumLobbyMembers(Id);

        /// <summary>
        /// 获取房间最大人数
        /// </summary>
        /// <returns>返回房间最大人数</returns>
        public int MaxMemberCount => SteamMatchmakingHelper.GetLobbyMemberLimit(Id);

        /// <summary>
        /// 获取房间拥有者
        /// </summary>
        /// <returns>返回房间拥有者的SteamID</returns>
        public CSteamID Owner => SteamMatchmakingHelper.GetLobbyOwner(Id);

        /// <summary>
        /// 设置房间密码
        /// </summary>
        /// <param name="password">要设置的房间密码</param>
        public void SetLobbyPassword(string password) =>
            SteamMatchmakingHelper.SetLobbyData(Id, PasswordKey, password);

        /// <summary>
        /// 获取房间密码
        /// </summary>
        /// <returns>返回房间密码</returns>
        public string GetLobbyPassword() =>
            SteamMatchmakingHelper.GetLobbyData(Id, PasswordKey);

        /// <summary>
        /// 检查房间是否设置了密码
        /// </summary>
        /// <returns>如果房间设置了密码则返回true，否则返回false</returns>
        public bool HasLobbyPassword() =>
            !string.IsNullOrEmpty(SteamMatchmakingHelper.GetLobbyData(Id, PasswordKey));

        /// <summary>
        /// 设置房间名称
        /// </summary>
        /// <param name="name">要设置的房间名称</param>
        public void SetLobbyName(string name) =>
            SteamMatchmakingHelper.SetLobbyData(Id, NameKey, name);

        /// <summary>
        /// 设置房间为公开
        /// </summary>
        public void SetPublic() =>
            SteamMatchmakingHelper.SetLobbyType(Id, ELobbyType.k_ELobbyTypePublic);

        /// <summary>
        /// 设置房间为私有
        /// </summary>
        public void SetPrivate() =>
            SteamMatchmakingHelper.SetLobbyType(Id, ELobbyType.k_ELobbyTypePrivate);

        public async Task<RoomEnter> Join()
        {
            var result = await SteamLobbyHelper.JoinLobby(Id);
            return (RoomEnter)result.m_EChatRoomEnterResponse;
        }

        public void Leave() => SteamMatchmakingHelper.LeaveLobby(Id);

        public void Refresh()
        {
            SteamMatchmakingHelper.RequestLobbyData(Id);
        }
        
        public void SetData(string key, string value)
        {
            SteamMatchmakingHelper.SetLobbyData(Id, key, value);
        }
        
        public string GetData(string key)
        {
            return SteamMatchmakingHelper.GetLobbyData(Id, key);
        }

        /// <summary>
        /// 表示房间进入的结果状态
        /// </summary>
        public enum RoomEnter
        {
            /// <summary>
            /// 成功进入房间
            /// </summary>
            Success = 1,

            /// <summary>
            /// 房间不存在
            /// </summary>
            DoesntExist = 2,

            /// <summary>
            /// 不允许进入房间
            /// </summary>
            NotAllowed = 3,

            /// <summary>
            /// 房间已满
            /// </summary>
            Full = 4,

            /// <summary>
            /// 发生错误
            /// </summary>
            Error = 5,

            /// <summary>
            /// 被禁止进入房间
            /// </summary>
            Banned = 6,

            /// <summary>
            /// 限制进入房间
            /// </summary>
            Limited = 7,

            /// <summary>
            /// 公会功能被禁用
            /// </summary>
            ClanDisabled = 8,

            /// <summary>
            /// 社区禁令
            /// </summary>
            CommunityBan = 9,

            /// <summary>
            /// 成员阻止了你
            /// </summary>
            MemberBlockedYou = 10, // 0x0000000A

            /// <summary>
            /// 你阻止了成员
            /// </summary>
            YouBlockedMember = 11, // 0x0000000B

            /// <summary>
            /// 超过速率限制
            /// </summary>
            RatelimitExceeded = 15, // 0x0000000F
        }
    }
}