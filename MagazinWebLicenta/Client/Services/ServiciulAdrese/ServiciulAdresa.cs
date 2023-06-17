namespace MagazinWebLicenta.Client.Services.ServiciulAdresa
{
	public class ServiciulAdresa : IServiciulAdresa
	{
		private readonly HttpClient http;

		public ServiciulAdresa(HttpClient http)
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
