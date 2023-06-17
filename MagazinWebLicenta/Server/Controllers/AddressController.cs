using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagazinWebLicenta.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AddressController : ControllerBase
	{
		private readonly IServiciulAdresa ServiciulAdresa;

		public AddressController(IServiciulAdresa ServiciulAdresa)
        {
			this.ServiciulAdresa = ServiciulAdresa;
		}

		[HttpGet]
		 public async Task<ActionResult<ServiceResponse<Address>>> GetAddress()
		{
			return await this.ServiciulAdresa.GetAddress();
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<Address>>> AddOrUpdateAddress(Address address)
		{
			return await this.ServiciulAdresa.AddOrUpdateAddress(address);
		}
	}
}
