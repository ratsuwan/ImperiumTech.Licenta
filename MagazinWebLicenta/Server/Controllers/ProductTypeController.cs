﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagazinWebLicenta.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            this.productTypeService = productTypeService;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProductTypes()
        {
            var response = await this.productTypeService.GetProductTypes();
            return Ok(response);
        }

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<List<ProductType>>>> AddProductType(ProductType productType)
		{
			var response = await this.productTypeService.AddProductType(productType);
			return Ok(response);
		}

		[HttpPut]
		public async Task<ActionResult<ServiceResponse<List<ProductType>>>> UpdateProductType(ProductType productType)
		{
			var response = await this.productTypeService.UpdateProductType(productType);
			return Ok(response);
		}
	}
}
