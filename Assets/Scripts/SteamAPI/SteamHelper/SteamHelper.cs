using System.Threading.Tasks;
using Steamworks;

namespace SteamAPI.SteamHelper
{
    public static class SteamHelper
    {
        public const string PasswordKey = "password"; // 密码键常量
        public const string NameKey = "name"; // 名称键常量
        public const string GameFilterKey = "YuoHira";
        public const string HouseOwnerKey = "HouseOwnerServerPost";
        public const string MemberIDKey = "NetID";
        public static async Task<T> WaitAsync<T>(this SteamAPICall_t apiCall)
        {
            var tcs = new TaskCompletionSource<T>();

            CallResult<T> callResult = new CallResult<T>();
            callResult.Set(apiCall, (param, bIOFailure) =>
            {
                if (bIOFailure)
                {
                    tcs.SetException(new System.Exception("IO Failure"));
                }
                else
                {
                    tcs.SetResult(param);
                }
            });

            return await tcs.Task;
        }
    }
}