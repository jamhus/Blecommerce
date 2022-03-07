using Blecommerce.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Blecommerce.Client.Shared
{
    public partial class ProductList
    {
        [Inject]
        protected HttpClient client { get; set; } = default!;

        private static List<Product> Products = new List<Product>();

        protected override async Task OnInitializedAsync()
        {
            var result = await client.GetFromJsonAsync<List<Product>>("api/product");
            if (result != null) Products = result;
        }
    }
}
