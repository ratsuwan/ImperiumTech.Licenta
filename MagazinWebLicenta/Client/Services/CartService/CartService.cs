using Blazored.LocalStorage;
using MagazinWebLicenta.Client.Pages;
using MagazinWebLicenta.Shared;
using static System.Net.WebRequestMethods;

namespace MagazinWebLicenta.Client.Services.CartService
{
	public class CartService : ICartService
	{
		private readonly ILocalStorageService localStorage;
		private readonly HttpClient http;
		private readonly IAuthService authService;

		public CartService(ILocalStorageService localStorage, HttpClient http,
			IAuthService authService)
        {
			this.localStorage = localStorage;
			this.http = http;
			this.authService = authService;
		}

        public event Action OnChange;

		public async Task AddToCart(CartItem cartItem)
		{
			if (await this.authService.IsUserAuthenticated())
			{
				await this.http.PostAsJsonAsync("api/cart/add", cartItem);
			}
			else
			{
				var cart = await this.localStorage.GetItemAsync<List<CartItem>>("cart");
				if (cart == null)
				{
					cart = new List<CartItem>();
				}

				var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId &&
				x.ProductTypeId == cartItem.ProductTypeId);
				if (sameItem == null)
				{
					cart.Add(cartItem);
				}
				else
				{
					sameItem.Quantity = cartItem.Quantity;
				}


				await this.localStorage.SetItemAsync("cart", cart);
			}
			await GetCartItemsCount();
		}

		

		public async Task GetCartItemsCount()
		{
			if (await this.authService.IsUserAuthenticated())
			{
				var result = await this.http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
				var count = result.Data;

				await this.localStorage.SetItemAsync<int>("cartItemsCount", count);
			}
			else
			{
				var cart = await this.localStorage.GetItemAsync<List<CartItem>>("cart");
				await this.localStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
			}
			OnChange.Invoke();
		}

		public async Task<List<CartProductResponse>> GetCartProducts()
		{
			if (await this.authService.IsUserAuthenticated())
			{
				var response = await this.http.GetFromJsonAsync<ServiceResponse<List<CartProductResponse>>>("api/cart");
				return response.Data;
			}
			else
			{
				var cartItems = await this.localStorage.GetItemAsync<List<CartItem>>("cart");
				if (cartItems != null)
					return new List<CartProductResponse>();
				var response = await this.http.PostAsJsonAsync("api/cart/products", cartItems);
				var cartProducts =
					await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();

				return cartProducts.Data;
			}
			
		}

		public async Task RemoveProductFromCart(int productId, int productTypeId)
		{
			if (await this.authService.IsUserAuthenticated())
			{
				await this.http.DeleteAsync($"api/cart/{productId}/{productTypeId}");
			}
			else
			{
				var cart = await this.localStorage.GetItemAsync<List<CartItem>>("cart");
				if (cart == null)
				{
					return;
				}
				var cartItem = cart.Find(x => x.ProductId == productId
					&& x.ProductTypeId == productTypeId);
				if (cartItem != null)
				{
					cart.Remove(cartItem);
					await this.localStorage.SetItemAsync("cart", cart);
				}
			}
		}

		public async Task StoreCartItems(bool emptyLocalCart = false)
		{
			var localCart = await this.localStorage.GetItemAsync<List<CartItem>>("cart");
			if (localCart == null)
			{
				return;
			}

			await this.http.PostAsJsonAsync("api/cart", localCart);

			if (emptyLocalCart)
			{
				await this.localStorage.RemoveItemAsync("cart");
			}
		}

		public async Task UpdateQuantity(CartProductResponse product)
		{
			if(await this.authService.IsUserAuthenticated())
			{
				var request = new CartItem
				{

					ProductId = product.ProductId,
					Quantity = product.Quantity,
					ProductTypeId = product.ProductTypeId
				};
				await this.http.PutAsJsonAsync("api/cart/update-quantity", request);
			}
			else
			{
				var cart = await this.localStorage.GetItemAsync<List<CartItem>>("cart");
				if (cart == null)
				{
					return;
				}
				var cartItem = cart.Find(x => x.ProductId == product.ProductId
					&& x.ProductTypeId == product.ProductTypeId);
				if (cartItem != null)
				{
					cartItem.Quantity = product.Quantity;
					await this.localStorage.SetItemAsync("cart", cart);

				}
			}

			
		}

		
	}
}
