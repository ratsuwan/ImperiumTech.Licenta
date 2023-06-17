namespace MagazinWebLicenta.Server.Services.ServiciulComenzi
{
	public interface IServiciulComenzi
	{
		Task<ServiceResponse<bool>> PlaceOrder(int userId);
		Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders();
		Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId);
	}
}
