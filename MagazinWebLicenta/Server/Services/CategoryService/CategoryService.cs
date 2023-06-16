using Microsoft.AspNetCore.Mvc.Routing;

namespace MagazinWebLicenta.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext context;

        public CategoryService(DataContext context)
        {
            this.context = context;
        }

		public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
		{
			category.Editing = category.IsNew = false;
			this.context.Categories.Add(category);
			await this.context.SaveChangesAsync();
			return await GetAdminCategories();
		}

		public async Task<ServiceResponse<List<Category>>> DeleteCategory(int id)
		{
			Category category = await GetCategoryById(id);
			if (category == null)
			{
				return new ServiceResponse<List<Category>>
				{
					Success = false,
					Message = "Categoria nu a fost găsită."
                };
			}

			category.Deleted = true;
			await this.context.SaveChangesAsync();

			return await GetAdminCategories();
		}

		private async Task<Category> GetCategoryById(int id)
		{
			return await this.context.Categories.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
		{
			var categories = await context.Categories
				.Where(c => !c.Deleted)
				.ToListAsync();
			return new ServiceResponse<List<Category>>
			{
				Data = categories,
			};
		}

		public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            var categories = await context.Categories
                .Where(c => !c.Deleted && c.Visible)
                .ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = categories,
            };
        }

		public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
		{
			var dbCategory = await GetCategoryById(category.Id);
			if(dbCategory == null)
			{
				return new ServiceResponse<List<Category>>
				{
					Success = false,
					Message = "Categoria nu a fost găsită."
                };
			}

			dbCategory.Name = category.Name;
			dbCategory.Url = category.Url;
			dbCategory.Visible = category.Visible;

			await this.context.SaveChangesAsync();

			return await GetAdminCategories();
		}
	}
}
