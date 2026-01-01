using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothStore_Backend.Data.Queries
{
    public class ProductVariantQuery
    {
        public string? Search { get; set; }

        public List<int>? ProductCategoryId { get; set; }
        public List<int>? GenderCategoryId { get; set; }

        public List<int>? ColorId { get; set; }
        public List<int>? SizeId { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string SortBy { get; set; } = "price";
        public string SortOrder { get; set; } = "asc";

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
