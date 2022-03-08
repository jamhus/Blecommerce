using Microsoft.AspNetCore.Components;

namespace Blecommerce.Client.Shared
{
    public partial class ProductList
    {
        [Inject]
        IProductService ProductService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await ProductService.GetProducts();
        }
    }
}
