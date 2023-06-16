using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagazinWebLicenta.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ICartService cartService;

		public CartController(ICartService cartService)
        {
			this.cartService = cartService;
		}


		[HttpPost("products")]
		public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartProducts(List<CartItem> cartItems)
		{
			var result = await cartService.GetCartProducts(cartItems);
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> StoreCartItems(List<CartItem> cartItems)
		{
			var result = await cartService.StoreCartItems(cartItems);
			return Ok(result);
		}

		[HttpPost("add")]
		public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem)
		{
			var result = await cartService.AddToCart(cartItem);
			return Ok(result);
		}

		[HttpPut("update-quantity")]
		public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(CartItem cartItem)
		{
			var result = await cartService.UpdateQuantity(cartItem);
			return Ok(result);
		}

		[HttpDelete("{productId}/{productTypeId}")]
		public async Task<ActionResult<ServiceResponse<bool>>> RemoveItemFromCart(int productId, int productTypeId)
		{
			var result = await cartService.RemoveItemFromCart(productId, productTypeId);
			return Ok(result);
		}

		[HttpGet("count")]

		public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
		{
			return await this.cartService.GetCartItemsCount();
		}

		[HttpGet]

		public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetDbCartProducts()
		{
			var result = await this.cartService.GetDbCartProducts();
			return Ok(result);
		}
	}
}
