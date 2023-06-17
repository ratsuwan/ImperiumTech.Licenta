namespace MagazinWebLicenta.Server.Services.ServiciulAdresa
{
	public interface IServiciulAdresa
	{
		Task<ServiceResponse<Address>> GetAddress();
		Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address);
	}
}
