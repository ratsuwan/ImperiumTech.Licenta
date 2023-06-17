using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagazinWebLicenta.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly IServiciulCosCumparaturi ServiciulCosCumparaturi;

		public CartController(IServiciulCosCumparaturi ServiciulCosCumparaturi)
        {
			this.ServiciulCosCumparaturi = ServiciulCosCumparaturi;
		}


		[HttpPost("products")]
		public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartProducts(List<CartItem> cartItems)
		{
			var result = await ServiciulCosCumparaturi.GetCartProducts(cartItems);
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> StoreCartItems(List<CartItem> cartItems)
		{
			var result = await ServiciulCosCumparaturi.StoreCartItems(cartItems);
			return Ok(result);
		}

		[HttpPost("add")]
		public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem)
		{
			var result = await ServiciulCosCumparaturi.AddToCart(cartItem);
			return Ok(result);
		}

		[HttpPut("update-quantity")]
		public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(CartItem cartItem)
		{
			var result = await ServiciulCosCumparaturi.UpdateQuantity(cartItem);
			return Ok(result);
		}

		[HttpDelete("{productId}/{productTypeId}")]
		public async Task<ActionResult<ServiceResponse<bool>>> RemoveItemFromCart(int productId, int productTypeId)
		{
			var result = await ServiciulCosCumparaturi.RemoveItemFromCart(productId, productTypeId);
			return Ok(result);
		}

		[HttpGet("count")]

		public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
		{
			return await this.ServiciulCosCumparaturi.GetCartItemsCount();
		}

		[HttpGet]

		public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetDbCartProducts()
		{
			var result = await this.ServiciulCosCumparaturi.GetDbCartProducts();
			return Ok(result);
		}
	}
}
