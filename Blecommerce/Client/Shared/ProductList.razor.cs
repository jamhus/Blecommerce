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

        internal string GetPriceText(Product product)
        {
            var variants = product.Variants;
            if (variants.Count == 0)
            {
                return string.Empty;
            }
            else if (variants.Count == 1)
            {
                return $"${variants[0].Price}";
            }

            decimal price = variants.Min(v => v.Price);
            return $"Starting at ${price}";

        }
    }
}
