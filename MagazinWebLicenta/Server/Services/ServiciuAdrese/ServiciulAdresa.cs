﻿namespace MagazinWebLicenta.Server.Services.ServiciulAdresa
{
	public class ServiciulAdresa : IServiciulAdresa
	{
		private readonly DataContext context;
		private readonly IServiciulAutentificari ServiciulAutentificari;

		public ServiciulAdresa(DataContext context, IServiciulAutentificari ServiciulAutentificari)
        {
			this.context = context;
			this.ServiciulAutentificari = ServiciulAutentificari;
		}

        public async Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
		{
			var response = new ServiceResponse<Address>();
			var dbAddress = (await GetAddress()).Data;
			if (dbAddress == null)
			{
				address.UserId = this.ServiciulAutentificari.GetUserId();
				this.context.Addresses.Add(address);
				response.Data = address;
			}
			else
			{
				dbAddress.FirstName =address.FirstName;
				dbAddress.LastName =address.LastName;
				dbAddress.State = address.State;
				dbAddress.Country = address.Country;
				dbAddress.City = address.City;
				dbAddress.Zip = address.Zip;
				dbAddress.Street = address.Street;
				response.Data = dbAddress;
			}
			await this.context.SaveChangesAsync();
			return response;
		}

		public async Task<ServiceResponse<Address>> GetAddress()
		{
			int userId = ServiciulAutentificari.GetUserId();
			var address = await this.context.Addresses
				.FirstOrDefaultAsync(a => a.UserId == userId);
			return new ServiceResponse<Address> { Data = address };
		}
	}
}
