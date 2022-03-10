namespace Blecommerce.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public CategoryService(HttpClient http)
        {
            _http = http;
        }

        public List<Category> categories { get; set; } = new List<Category>();

        public async Task GetCategories()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");
            if (result != null && result.Data != null)
                categories = result.Data;
        }

        public async Task<ServiceResponse<Category>> GetCategory(int id)
        {

            var result = await _http.GetFromJsonAsync<ServiceResponse<Category>>($"api/product/{id}");
            if (result != null && result.Data != null)
                return result;
            return new ServiceResponse<Category>();
        }
    }
}
