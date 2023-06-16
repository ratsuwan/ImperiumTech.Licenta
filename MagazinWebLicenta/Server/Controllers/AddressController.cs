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
		private readonly IAddressService addressService;

		public AddressController(IAddressService addressService)
        {
			this.addressService = addressService;
		}

		[HttpGet]
		 public async Task<ActionResult<ServiceResponse<Address>>> GetAddress()
		{
			return await this.addressService.GetAddress();
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<Address>>> AddOrUpdateAddress(Address address)
		{
			return await this.addressService.AddOrUpdateAddress(address);
		}
	}
}
