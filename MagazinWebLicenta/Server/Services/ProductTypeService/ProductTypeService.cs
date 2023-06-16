using System.Runtime.InteropServices;

namespace MagazinWebLicenta.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext context;

        public ProductTypeService(DataContext context)
        {
            this.context = context;
        }

		public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
		{
			productType.Editing = productType.IsNew = false;
			this.context.ProductTypes.Add(productType);
            await this.context.SaveChangesAsync();

            return await GetProductTypes();
		}

		public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await this.context.ProductTypes.ToListAsync();
            return new ServiceResponse<List<ProductType>> { Data =  productTypes };

        }

		public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
		{
			var dbProductType = await this.context.ProductTypes.FindAsync(productType.Id);
            if(dbProductType == null)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Product type not found."
                };
            }
            dbProductType.Name = productType.Name;
            await this.context.SaveChangesAsync();

            return await GetProductTypes();
		}
	}
}
