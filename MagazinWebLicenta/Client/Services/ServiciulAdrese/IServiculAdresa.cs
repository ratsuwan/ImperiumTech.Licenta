namespace MagazinWebLicenta.Client.Services.ServiciulAdresa
{
	public interface IServiciulAdresa
	{
		Task<Address> GetAddress();
		Task<Address> AddOrUpdateAddress(Address address);
	}
}
