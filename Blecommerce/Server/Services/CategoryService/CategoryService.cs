namespace Blecommerce.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            var response = new ServiceResponse<List<Category>>()
            {
                Data = categories
            };
            return response;
        }

        public async Task<ServiceResponse<Category>> GetCategoryAsync(int id)
        {

            var category = await _context.Categories.FindAsync(id);
            var response = new ServiceResponse<Category>();
            if (category == null)
            {
                response.Success = false;
                response.Message = "Sorry , this Category doesn't exist";
            }
            else
            {
                response.Data = category;
            }
            return response;
        }
    }
}
