using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagazinWebLicenta.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
	{
        
        private readonly IServiciulProduse ServiciulProduse;

        public ProductController(IServiciulProduse ServiciulProduse)
        {
            
            this.ServiciulProduse = ServiciulProduse;
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts()
		{
			var result = await this.ServiciulProduse.GetAdminProducts();
			return Ok(result);
		}

		[HttpPost, Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<List<Product>>>> CreateProduct(Product product)
		{
			var result = await this.ServiciulProduse.CreateProduct(product);
			return Ok(result);
		}

		[HttpPut, Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<List<Product>>>> UpdateProduct(Product product)
		{
			var result = await this.ServiciulProduse.UpdateProduct(product);
			return Ok(result);
		}

		[HttpDelete("{id}"), Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
		{
			var result = await this.ServiciulProduse.DeleteProduct(id);
			return Ok(result);
		}

		[HttpGet]
		public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var result = await this.ServiciulProduse.GetProductsAsync();
			return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
        {
            var result = await this.ServiciulProduse.GetProductAsync(productId);
            return Ok(result);
        }

        [HttpGet("category/{categoryUrl}")]

        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl)
        {
            var result = await this.ServiciulProduse.GetProductsByCategory(categoryUrl);
            return Ok(result);
        }


        [HttpGet("search/{searchText}/{page}")]

        public async Task<ActionResult<ServiceResponse<ProductSearchResult>>> SearchProducts(string searchText, int page =1 )
        {
            var result = await this.ServiciulProduse.SearchProducts(searchText, page);
            return Ok(result);
        }

        [HttpGet("searchsuggestions/{searchText}")]

        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestions(string searchText)
        {
            var result = await this.ServiciulProduse.GetProductSearchSuggestions(searchText);
            return Ok(result);
        }

        [HttpGet("featured")]

        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
        {
            var result = await this.ServiciulProduse.GetFeaturedProducts();
            return Ok(result);
        }
    }
}
