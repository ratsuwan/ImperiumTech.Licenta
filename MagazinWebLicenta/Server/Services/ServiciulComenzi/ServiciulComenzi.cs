using System.Security.Claims;
using MagazinWebLicenta.Shared;

namespace MagazinWebLicenta.Server.Services.ServiciulComenzi
{
	public class ServiciulComenzi : IServiciulComenzi
	{
		private readonly DataContext context;
		private readonly IServiciulCosCumparaturi cartService;
		private readonly IServiciulAutentificari authService;
		private readonly IHttpContextAccessor httpContextAccessor;

		public ServiciulComenzi(DataContext context,
			IServiciulCosCumparaturi cartService,
			IServiciulAutentificari authService)
        {
			this.context = context;
			this.cartService = cartService;
			this.authService = authService;
		}

		public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders()
		{
			var response = new ServiceResponse<List<OrderOverviewResponse>>();
			var orders = await this.context.Orders
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product)
				.Where(o => o.UserId == this.authService.GetUserId())
				.OrderByDescending(o => o.OrderDate)
				.ToListAsync();

			var orderResponse = new List<OrderOverviewResponse>();
			orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse 
			{ 
				Id = o.Id,
				OrderDate = o.OrderDate,
				TotalPrice = o.TotalPrice,
				Product = o.OrderItems.Count > 1 ?
				  $"{o.OrderItems.First().Product.Title} si inca " +
				  $"{o.OrderItems.Count -1} produs(e)..." :
				  o.OrderItems.First().Product.Title,
				ProductImageUrl = o.OrderItems.First().Product.ImageUrl
			
			}));

			response.Data = orderResponse;
			return response;
		}

		public async Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId)
		{
			var response = new ServiceResponse<OrderDetailsResponse>();
			var order = await this.context.Orders
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product)
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.ProductType)
				.Where( o => o.UserId == this.authService.GetUserId() && o.Id == orderId)
				.OrderByDescending(o => o.OrderDate)
				.FirstOrDefaultAsync();

			if (order == null)
			{
				response.Success =false;
				response.Message = "Order not found.";
					return response;
			}

			var orderDetailsResponse = new OrderDetailsResponse
			{
				OrderDate = order.OrderDate,
				TotalPrice = order.TotalPrice,
				Products = new List<OrderDetailsProductResponse>()
			};

			order.OrderItems.ForEach(item =>
			orderDetailsResponse.Products.Add(new OrderDetailsProductResponse  {
				ProductId = item.ProductId,
				ImageUrl = item.Product.ImageUrl,
				ProductType =item.ProductType.Name,
				Quantity = item.Quantity,
				Title = item.Product.Title,
				TotalPrice = item.TotalPrice
			}));

			response.Data = orderDetailsResponse;
			return response;
		}

		public async Task<ServiceResponse<bool>> PlaceOrder(int userId)
		{
			var products = (await this.cartService.GetDbCartProducts(userId)).Data;
			decimal totalPrice = 0;
			products.ForEach(product => totalPrice += product.Price * product.Quantity);

			var orderItems = new List<OrderItem>();
			products.ForEach(product => orderItems.Add(new OrderItem
			{
				ProductId = product.ProductId,
				ProductTypeId = product.ProductTypeId,
				Quantity = product.Quantity,
				TotalPrice = product.Price * product.Quantity
			}));

			var order = new Order
			{
				UserId = userId,
				OrderDate = DateTime.UtcNow,
				TotalPrice = totalPrice,
				OrderItems = orderItems

			};

			this.context.Orders.Add(order);

			this.context.CartItems.RemoveRange(this.context.CartItems
				.Where(ci => ci.UserId == userId));

			await this.context.SaveChangesAsync();

			return new ServiceResponse<bool> { Data = true };


		}
	}
}
