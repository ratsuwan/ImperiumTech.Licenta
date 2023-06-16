using Microsoft.AspNetCore.Components;

namespace MagazinWebLicenta.Client.Services.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly HttpClient http;
		private readonly AuthenticationStateProvider authStateProvider;
		private readonly NavigationManager navigationManager;

		public OrderService(HttpClient http,
			AuthenticationStateProvider authStateProvider,
			NavigationManager navigationManager)
        {
			this.http = http;
			this.authStateProvider = authStateProvider;
			this.navigationManager = navigationManager;
		}

		public async Task<OrderDetailsResponse> GetOrderDetails(int orderId)
		{
			var result = await this.http.GetFromJsonAsync<ServiceResponse<OrderDetailsResponse>>($"api/order/{orderId}");
			return result.Data;
		}

		public async Task<List<OrderOverviewResponse>> GetOrders()
		{
			var result = await this.http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResponse>>>("api/order");
			return result.Data;
		}

		public async Task<string> PlaceOrder()
		{
			if(await IsUserAuthenticated())
			{
				var result = await this.http.PostAsync("api/payment/checkout", null);
				var url = await result.Content.ReadAsStringAsync();
				return url;

			}
			else
			{
				return "login";
			}
		}

		private async Task<bool> IsUserAuthenticated()
		{
			return (await this.authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
		}
	}
}
