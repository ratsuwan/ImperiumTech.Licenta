namespace MagazinWebLicenta.Server.Services.ServiciulCategorii
{
    public interface IServiciulCategorii
    {
        Task<ServiceResponse<List<Category>>> GetCategories();
        Task<ServiceResponse<List<Category>>> GetAdminCategories();
		Task<ServiceResponse<List<Category>>> AddCategory(Category category);
		Task<ServiceResponse<List<Category>>> UpdateCategory(Category category);
		Task<ServiceResponse<List<Category>>> DeleteCategory(int id);

	}
}
