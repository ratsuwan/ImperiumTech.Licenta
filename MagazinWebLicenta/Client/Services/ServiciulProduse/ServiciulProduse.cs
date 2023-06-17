

using System.Runtime.InteropServices;

namespace MagazinWebLicenta.Client.Services.ServiciulProduse
{
    public class ServiciulProduse : IServiciulProduse
    {
        private readonly HttpClient http;

        public ServiciulProduse(HttpClient http)
        {
            this.http = http;
        }

        public List<Product> Products { get; set; } = new List<Product>();
        public string Message { get; set; } = "Loading Products...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;
		public List<Product> AdminProducts { get; set ; }

		public event Action ProductsChanged;

		public async Task<Product> CreateProduct(Product product)
		{
            var result = await this.http.PostAsJsonAsync("api/product", product);
            var newProduct = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
            return newProduct;
		}

		public async Task DeleteProduct(Product product)
		{
            var result = await this.http.DeleteAsync($"api/product/{product.Id}");
		}

		public async Task GetAdminProducts()
		{
            var result = await this.http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/admin");
            AdminProducts = result.Data;
            CurrentPage = 1;
            PageCount = 0;
            if (AdminProducts.Count == 0)
                Message = "No products found.";
		}

		public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await this.http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return result;
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = categoryUrl == null ?
                await this.http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured"):
                await this.http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
            if (result != null && result.Data != null)
                Products = result.Data;
            CurrentPage = 1;
            PageCount = 0;

            if (Products.Count == 0)
                Message = "No products found";
            

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSeachSuggestions(string searchText)
        {
            var result = await this.http
                .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");
            return result.Data;
        }


        public async Task SearchProducts(string searchText, int page)
        {
            LastSearchText = searchText;
            var result = await this.http
                .GetFromJsonAsync<ServiceResponse<ProductSearchResult>>($"api/product/search/{searchText}/{page}");
            if (result != null && result.Data != null)
            {

                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            if (Products.Count == 0) Message = "No products found.";
            ProductsChanged?.Invoke();

        }

		public async Task<Product> UpdateProduct(Product product)
		{
            var result = await this.http.PutAsJsonAsync($"api/product", product);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
		}
	}
}
