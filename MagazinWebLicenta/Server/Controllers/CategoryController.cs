using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagazinWebLicenta.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IServiciulCategorii ServiciulCategorii;

        public CategoryController(IServiciulCategorii ServiciulCategorii)
        {
            this.ServiciulCategorii = ServiciulCategorii;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategories()
        {
            var result = await ServiciulCategorii.GetCategories();
            return Ok(result);
        }

		[HttpGet("admin"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<List<Category>>>> GetAdminCategories()
		{
			var result = await ServiciulCategorii.GetAdminCategories();
			return Ok(result);
		}

		[HttpDelete("admin/{id}"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<List<Category>>>> DeleteCategory(int id)
		{
			var result = await ServiciulCategorii.DeleteCategory(id);
			return Ok(result);
		}

		[HttpPost("admin"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<List<Category>>>> AddCategory(Category category)
		{
			var result = await ServiciulCategorii.AddCategory(category);
			return Ok(result);
		}

		[HttpPut("admin"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<List<Category>>>> UpdateCategory(Category category)
		{
			var result = await ServiciulCategorii.UpdateCategory(category);
			return Ok(result);
		}
	}
}
