namespace MagazinWebLicenta.Client.Services.ServiciulCategorii
{
    public interface IServiciulCategorii
    {
        event Action OnChange;

        List<Category>  Categories { get; set; }
		List<Category> AdminCategories { get; set; }

		Task GetCategories();

        Task GetAdminCategories();
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int categoryId);
        Category CreateNewCategory();
    }
}
