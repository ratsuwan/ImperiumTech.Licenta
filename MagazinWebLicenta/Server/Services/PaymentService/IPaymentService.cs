

using Stripe.Checkout;

namespace MagazinWebLicenta.Server.Services.PaymentService
{
	public interface IPaymentService
	{
		Task<Session> CreateCheckoutSession();

		Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request);
	}
}
