using Microsoft.AspNetCore.Components;

namespace Blecommerce.Client.Shared
{
    public partial class ProductList
    {
        [Inject]
        IProductService ProductService { get; set; } = default!;

        protected override void OnInitialized()
        {
            ProductService.ProductsChanged += StateHasChanged;
        }

        public void Dispose()
        {
            ProductService.ProductsChanged -= StateHasChanged;
        }
    }
}
