namespace Blecommerce.Server.Extenstions
{
    public static class ProductExtensions
    {
        public static IQueryable<Product> IsAdmin(this IQueryable<Product> query, bool admin)
        {
            if (admin)
            {
                query = query.Where(p => !p.Deleted)
                 .Include(p => p.Variants.Where(v => !v.Deleted))
                 .ThenInclude(v => v.ProductType);
            }
            else
            {

                query = query.Where(p => p.Visible && !p.Deleted)
                                  .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted));
            }

            return query;
        }
        public static IQueryable<Product> Search(this IQueryable<Product> query, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue)) return query;

            var searchValueLowerCase = searchValue.Trim().ToLower();

            query = query.Where(p => p.Title.ToLower().Contains(searchValueLowerCase.ToLower()) ||
                                    p.Description.ToLower().Contains(searchValueLowerCase.ToLower()));
                                   
            return query;
        }

        public static IQueryable<Product> Filter(this IQueryable<Product> query, string categoryUrls, bool admin = false)
        {
            if (string.IsNullOrEmpty(categoryUrls)) return query;

            var categories = new List<string>();


            if (!string.IsNullOrEmpty(categoryUrls))
            {
                categories.AddRange(categoryUrls.Split(",").ToList());
            }

            query = query.Where(p => categories.Contains(p.Category.Url.ToLower()));

            return query;
        }
    }
}
