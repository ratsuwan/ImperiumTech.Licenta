using System.Security.Claims;
using MagazinWebLicenta.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MagazinWebLicenta.Server.Services.CartService
{
	public class CartService : ICartService
	{
		private DataContext _dataContext;
		private readonly IAuthService authService;

		public CartService(DataContext context, IAuthService authService)
        {
			Context = context;
			this.authService = authService;
		}


		public DataContext Context { get; }

		public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems)
		{
			var result = new ServiceResponse<List<CartProductResponse>>
			{
				Data = new List<CartProductResponse>()
			};

			foreach (var item in cartItems) 
			{
				var product = await this.Context.Products
					.Where(p => p.Id == item.ProductId)
					.Include(i => i.Images)
					.FirstOrDefaultAsync();

				if (product == null)
				{
					continue;
				}


				var productVariant = await this.Context.ProductVariants
					.Where(v => v.ProductId == item.ProductId
					&& v.ProductTypeId == item.ProductTypeId)
					.Include(v => v.ProductType)
					.FirstOrDefaultAsync();

				if (productVariant == null)
				{
					continue;
				}

				var url = product.ImageUrl;
				if (string.IsNullOrEmpty(url))
				{
					url = product.Images[0].Data;
				}

				var cartProduct = new CartProductResponse
				{
					ProductId = product.Id,
					Title = product.Title,
					ImageUrl = product.ImageUrl,
					Price = productVariant.Price,
					ProductType = productVariant.ProductType.Name,
					ProductTypeId = productVariant.ProductTypeId,
					Quantity = item.Quantity
				};

				result.Data.Add(cartProduct);
			}
			return result;	
		}

		public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems)
		{
			cartItems.ForEach(cartItem => cartItem.UserId = this.authService.GetUserId());
			this.Context.AddRange(cartItems);
			await this.Context.SaveChangesAsync();

			return await GetDbCartProducts();

		}
		public async Task<ServiceResponse<int>> GetCartItemsCount()
		{
			var count =(await this.Context.CartItems .Where(ci => ci.UserId == this.authService.GetUserId()).ToListAsync()).Count;
			return new ServiceResponse<int> {Data = count};
		}

		public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts(int? userId = null)
		{
			if(userId == null)	
				userId = this.authService.GetUserId();


			return await GetCartProducts(await this.Context.CartItems
				.Where(ci => ci.UserId== userId).ToListAsync());
		}

		public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
		{
			cartItem.UserId = this.authService.GetUserId();

			var sameItem = await this.Context.CartItems
				.FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
				ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == cartItem.UserId);
			if (sameItem == null)
			{
				this.Context.CartItems.Add(cartItem);
			}
			else
			{
				sameItem.Quantity += cartItem.Quantity;
			}

			await this.Context.SaveChangesAsync();
			return new ServiceResponse<bool> { Data = true };
		}

		public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
		{
			var dbCartItem = await this.Context.CartItems
				.FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
				ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == this.authService.GetUserId());
			if (dbCartItem == null)
			{
				return new ServiceResponse<bool>
				{
					Data = false,
					Success = false,
					Message = "Articolul din coș nu există."
                };
			}

			dbCartItem.Quantity = cartItem.Quantity;
			await this.Context.SaveChangesAsync();	
			return new ServiceResponse<bool> { Data= true };
		}

		public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
		{
			var dbCartItem = await this.Context.CartItems
				.FirstOrDefaultAsync(ci => ci.ProductId == productId &&
				ci.ProductTypeId == productTypeId && ci.UserId == this.authService.GetUserId());
			if (dbCartItem == null)
			{
				return new ServiceResponse<bool>
				{
					Data = false,
					Success = false,
					Message = "Articolul din coș nu există."
                };
			}

			this.Context.CartItems.Remove(dbCartItem);
			await this.Context.SaveChangesAsync();

			return new ServiceResponse<bool> { Data = true };
		}
	}
}
