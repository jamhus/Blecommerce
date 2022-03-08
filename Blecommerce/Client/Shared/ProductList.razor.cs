using Blecommerce.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

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
