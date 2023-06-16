namespace MagazinWebLicenta.Client.Services.AuthService
{
	public class AuthService : IAuthService
	{
		private readonly HttpClient http;
		private readonly AuthenticationStateProvider authStateProvider;

		public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
			this.http = http;
			this.authStateProvider = authStateProvider;
		}

		public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
		{
			var result = await this.http.PostAsJsonAsync("api/auth/change-password", request.Password);
			return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
		}

		public async Task<bool> IsUserAuthenticated()
		{
			return (await this.authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
		}

		public async Task<ServiceResponse<string>> Login(UserLogin request)
		{
			var result = await this.http.PostAsJsonAsync("api/auth/login", request);
			return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
		}

		public async Task<ServiceResponse<int>> Register(UserRegister request)
		{
			var result = await this.http.PostAsJsonAsync("api/auth/register", request);
			return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
		}
	}
}
