

using Stripe.Checkout;

namespace MagazinWebLicenta.Server.Services.ServiciulPlati
{
	public interface IServiciulPlati
	{
		Task<Session> CreateCheckoutSession();

		Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request);
	}
}
