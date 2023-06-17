using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagazinWebLicenta.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IServiciulAutentificari ServiciulAutentificari;

		public AuthController(IServiciulAutentificari ServiciulAutentificari)
		{
			this.ServiciulAutentificari = ServiciulAutentificari;
		}

		[HttpPost("register")]
		public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
		{
			var response = await this.ServiciulAutentificari.Register(
				new User
				{
					Email = request.Email
				},
			    request.Password);

			if(!response.Success)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}

		[HttpPost("Login")]
		public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
		{
			var response = await this.ServiciulAutentificari.Login(request.Email, request.Password);
			if(!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}

		[HttpPost("change-password"), Authorize]
		public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var response = await this.ServiciulAutentificari.ChangePassword(int.Parse(userId), newPassword);

			if(!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}
	}
}
