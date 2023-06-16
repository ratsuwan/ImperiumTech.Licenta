namespace MagazinWebLicenta.Client.Services.AddressService
{
	public class AddressService : IAddressService
	{
		private readonly HttpClient http;

		public AddressService(HttpClient http)
        {
			this.http = http;
		}
        public async Task<Address> AddOrUpdateAddress(Address address)
		{
			var response = await this.http.PostAsJsonAsync("api/address", address);
			return response.Content
				.ReadFromJsonAsync<ServiceResponse<Address>>().Result.Data;
		}

		public async Task<Address> GetAddress()
		{
			var response = await this.http.GetFromJsonAsync<ServiceResponse<Address>>("api/address");
			return response.Data;
		}
	}
}
