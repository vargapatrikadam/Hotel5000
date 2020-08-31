using BlazorWebApp.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebApp
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private static readonly TimeSpan UserCacheRefreshInterval = TimeSpan.FromSeconds(60);

        private readonly HttpClient _httpClient;

        private DateTimeOffset _userLastCheck = DateTimeOffset.FromUnixTimeSeconds(0);
        private ClaimsPrincipal _cachedUser = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return new AuthenticationState(await GetUser(useCache: true));
        }

        private async ValueTask<ClaimsPrincipal> GetUser(bool useCache = false)
        {
            var now = DateTimeOffset.Now;
            if (useCache && now < _userLastCheck + UserCacheRefreshInterval)
            {
                return _cachedUser;
            }

            _cachedUser = await RefreshToken();
            _userLastCheck = now;

            return _cachedUser;
        }

        private async Task<ClaimsPrincipal> RefreshToken()
        {
            UserInfo user = null;
           
            user = await _httpClient.GetFromJsonAsync<UserInfo>("api/auth/refresh");

            if (user == null || !user.IsAuthenticated)
            {
                return null;
            }

            var identity = new ClaimsIdentity(
                nameof(CustomAuthStateProvider),
                user.Username,
                user.Role);

            if (user.Claims != null)
            {
                foreach (var claim in user.Claims)
                {
                    identity.AddClaim(new Claim(claim.Type, claim.Value));
                }
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.AccessToken);

            return new ClaimsPrincipal(identity);
        }
    }
}
