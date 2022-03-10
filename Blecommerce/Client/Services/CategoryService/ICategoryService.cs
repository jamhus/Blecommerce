namespace Blecommerce.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        List<Category> categories { get; set; }
        Task GetCategories();
        Task<ServiceResponse<Category>> GetCategory(int id);
    }
}
