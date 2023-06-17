using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagazinWebLicenta.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IServiciulPlati ServiciulPlati;

		public PaymentController(IServiciulPlati ServiciulPlati)
        {
			this.ServiciulPlati = ServiciulPlati;
		}



		[HttpPost("checkout"), Authorize]
		public async Task<ActionResult<string>> CreateCheckoutSession()
		{
			var session = await this.ServiciulPlati.CreateCheckoutSession();
			return Ok(session.Url);
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<bool>>> FulfillOrder()
		{
			var response = await this.ServiciulPlati.FulfillOrder(Request);
			if(!response.Success)
				return BadRequest(response.Message);
			
			return Ok(response);
		}
	}
}
