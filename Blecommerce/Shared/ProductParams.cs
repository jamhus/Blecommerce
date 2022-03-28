using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blecommerce.Shared
{
    public class ProductParams : PaginationParams
    {
        public string? SearchText { get; set; }
        public string? Categories { get; set; }
    }
}
