namespace Blecommerce.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
        {
            category.Editing = category.ISNew = false;
            _context.Add(category);
            await _context.SaveChangesAsync();
            return await GetAdminCategories();

        }
        public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
        {
            Category dbcategory = await GetCategoryById(category.Id);
            if (category == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found"
                };
            }
            dbcategory.Name = category.Name;
            dbcategory.Url = category.Url;
            dbcategory.Visible = category.Visible;
            dbcategory.Icon = category.Icon;
            await _context.SaveChangesAsync();
            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategory(int id)
        {
            Category category = await GetCategoryById(id);
            if(category == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found"
                };
            }

            category.Deleted = true;
            await _context.SaveChangesAsync();
            return await GetAdminCategories();

        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
        {
            var categories = await _context.Categories
                .Where(c => !c.Deleted)
                .ToListAsync();
            var response = new ServiceResponse<List<Category>>()
            {
                Data = categories
            };
            return response;
        }

        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            var categories = await _context.Categories
                .Where(c=> !c.Deleted && c.Visible)
                .ToListAsync();
            var response = new ServiceResponse<List<Category>>()
            {
                Data = categories
            };
            return response;
        }

        private async Task<Category> GetCategoryById(int id) => await _context.Categories.FindAsync(id);

    }
}
