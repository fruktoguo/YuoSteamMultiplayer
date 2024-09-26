using System.Threading.Tasks;
using Steamworks;

namespace SteamAPI.SteamHelper
{
    public static class SteamHelper
    {
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