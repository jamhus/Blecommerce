namespace Blecommerce.Client.Services.ProductTypesService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient _http;

        public ProductTypeService(HttpClient http)
        {
            _http = http;
        }
        public List<ProductType> ProductTypes { get; set; }

        public event Action OnChange;

        public async Task AddProductType(ProductType productType)
        {
            var response = await _http.PostAsJsonAsync("api/producttype",productType);
            ProductTypes = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }

        public ProductType CreateNewProductType()
        {
            var newType = new ProductType { ISNew = true, Editing = true};
            ProductTypes.Add(newType);
            OnChange.Invoke();
            return newType;
        }

        public async Task GetProductTypes()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/productType");
            if (result != null && result.Data != null)
                ProductTypes = result.Data;
        }

        public async Task UpdateProductType(ProductType productType)
        {
            var response = await _http.PutAsJsonAsync("api/producttype", productType);
            ProductTypes = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }
    }
}
