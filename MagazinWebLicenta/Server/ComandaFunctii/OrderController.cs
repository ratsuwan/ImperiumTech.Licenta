using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagazinWebLicenta.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IServiciulComenzi ServiciulComenzi;

		public OrderController(IServiciulComenzi ServiciulComenzi)
        {
			this.ServiciulComenzi = ServiciulComenzi;
		}


		[HttpGet]
		public async Task<ActionResult<ServiceResponse<List<OrderOverviewResponse>>>> GetOrders()
		{
			var result = await this.ServiciulComenzi.GetOrders();
			return Ok(result);
		}

		[HttpGet("{orderId}")]
		public async Task<ActionResult<ServiceResponse<OrderDetailsResponse>>> GetOrdersDetails(int orderId)
		{
			var result = await this.ServiciulComenzi.GetOrderDetails(orderId);
			return Ok(result);
		}
	}
}
