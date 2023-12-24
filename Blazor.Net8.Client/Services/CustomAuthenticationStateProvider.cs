using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using SignalR.Server.Models;

namespace Blazor.Net8.Client.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly ProtectedSessionStorage _protectedSessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ProtectedLocalStorage protectedLocalStorage,
            ProtectedSessionStorage protectedSessionStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
            _protectedSessionStorage = protectedSessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _protectedSessionStorage.GetAsync<LoginModel>(AuthenticationConstants.CustomAuthFromCookie);
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
                if (userSession == null)
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    //new Claim(ClaimTypes.Role, userSession.Role)
                }, AuthenticationConstants.CustomAuthFromCookie));
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public async Task UpdateAuthenticationState(UserDataModel userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession != null)
            {
                await _protectedSessionStorage.SetAsync(AuthenticationConstants.CustomAuthFromCookie, userSession);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    //new Claim(ClaimTypes.Role, userSession.Role)
                }, AuthenticationConstants.CustomAuthFromCookie));
            }
            else
            {
                await _protectedSessionStorage.DeleteAsync(AuthenticationConstants.CustomAuthFromCookie);
                claimsPrincipal = _anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task<LoginModel> GetUserData()
        {
            LoginModel model = new LoginModel();
            var result = await _protectedSessionStorage.GetAsync<LoginModel>(AuthenticationConstants.CustomAuthFromCookie);
            if (result.Success)
            {
                model = result.Value;
            }
            return model;
        }
    }
    public class AuthenticationConstants
    {
        public static string UserSessionFromCookie { get; } = "952e39ba6f2270d755462ecda5246ae570370ad245d8eed27c5d8c6c05aadc9c";
        public static string CustomAuthFromCookie { get; } = "CustomAuth";
        public static string UserData { get; } = "UserData";
    }
}
