namespace MagazinWebLicenta.Client.Services.ServiciulComenzi
{
	public interface IServiciulComenzi
	{
		Task<string> PlaceOrder();
		Task<List<OrderOverviewResponse>> GetOrders();
		Task<OrderDetailsResponse> GetOrderDetails(int orderId);
	}
}
