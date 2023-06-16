using Stripe;
using Stripe.Checkout;

namespace MagazinWebLicenta.Server.Services.PaymentService
{
	public class PaymentService : IPaymentService
	{
		private readonly ICartService cartService;
		private readonly IAuthService authService;
		private readonly IOrderService orderService;

		const string secret = "whsec_b7f4914b81d8b55515fe8dbf34634803e01d4202b24e7b7f5351991c9ddb883b";

		public PaymentService(ICartService cartService,
			IAuthService authService,
			IOrderService orderService)
        {
			StripeConfiguration.ApiKey = "sk_test_51Mt8u3BuzkITudG44yDU4U5uZcfKyxrryDiDDT2Ey06owjq6lT84JdGb8X6DB7dJCdV7jJlYJ6vVXhjhx7C8YsFh00jgbpP6zT";

			this.cartService = cartService;
			this.authService = authService;
			this.orderService = orderService;
		}

        public async Task<Session> CreateCheckoutSession()
		{
			var products = (await this.cartService.GetDbCartProducts()).Data;
			var lineItems = new List<SessionLineItemOptions>();
			products.ForEach(product => lineItems.Add(new SessionLineItemOptions
			{
				PriceData = new SessionLineItemPriceDataOptions
				{
					UnitAmountDecimal = product.Price * 100,
					Currency = "ron",
					ProductData = new SessionLineItemPriceDataProductDataOptions
					{
						Name = product.Title,
						Images = new List<string> { product.ImageUrl }
					}
				},
				Quantity = product.Quantity
			}));

			var options = new SessionCreateOptions
			{
				CustomerEmail = this.authService.GetUserEmail(),
				ShippingAddressCollection =
				new SessionShippingAddressCollectionOptions
				{
					AllowedCountries = new List<string> { "RO" }
				},
				PaymentMethodTypes = new List<string>
				{
					"card"
				},
				LineItems = lineItems,
				Mode = "payment",
				SuccessUrl = "https://localhost:7017/order-success",
				CancelUrl = "https://localhost:7017/cart"
			};

			var service = new SessionService();
			Session session = service.Create(options);
			return session;
		}

		public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request)
		{
		     var json = await new StreamReader(request.Body).ReadToEndAsync();
			try 
			{
				var stripeEvent = EventUtility.ConstructEvent(
					json,
					request.Headers["Stripe-Signature"],
					secret
					);

				if(stripeEvent.Type == Events.CheckoutSessionCompleted)
				{
					var session = stripeEvent.Data.Object as Session;
					var user = await this.authService.GetUserByEmail(session.CustomerEmail);
					await this.orderService.PlaceOrder(user.Id);
				}

				return new ServiceResponse<bool> { Data = true };
			}

			catch (StripeException e) 
			{
				return new ServiceResponse<bool> { Data = false, Success = false, Message = e.Message };
			}
		
		}


			
	} 

}
