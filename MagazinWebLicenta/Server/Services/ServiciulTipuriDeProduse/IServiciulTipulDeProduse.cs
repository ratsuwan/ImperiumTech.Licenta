namespace MagazinWebLicenta.Server.Services.ServiciulTipuriDeProduse
{
    public interface IServiciulTipulDeProduse
    {
        Task<ServiceResponse<List<ProductType>>> GetProductTypes();
		Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType);
		Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType);

	}
}
