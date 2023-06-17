using System.Runtime.CompilerServices;

namespace MagazinWebLicenta.Client.Services.ServiciulTipuriDeProduse
{
    public class ServiciulTipuriDeProduse : IServiciulTipuriDeProduse
    {
        private readonly HttpClient http;

        public ServiciulTipuriDeProduse(HttpClient http)
        {
            this.http = http;
        }
        public List<ProductType> ProductTypes { get ; set; } = new List<ProductType>();

        public event Action OnChange;

		public async Task AddProductType(ProductType productType)
		{
			var response = await this.http.PostAsJsonAsync("api/producttype", productType);
			ProductTypes = (await response.Content
				.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
			OnChange.Invoke();
		}

		public ProductType CreateNewProductType()
		{
			var newProductType = new ProductType { IsNew = true, Editing = true };

			ProductTypes.Add(newProductType);
			OnChange.Invoke();
			return newProductType;
		}

		public async Task GetProductTypes()
        {
            var result = await this.http
                .GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/producttype");
            ProductTypes = result.Data;
        }

		public async Task UpdateProductType(ProductType productType)
		{
			var response = await this.http.PutAsJsonAsync("api/producttype", productType);
			ProductTypes = (await response.Content
				.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
			OnChange.Invoke();
		}
	}
}
