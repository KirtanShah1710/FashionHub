namespace ClothStore_Backend.Application.DTOs.RequestDto
{
    public class ProductVariantQueryDto
    {
        // search
        public string? Search { get; set; }

        // product level filtering (by category and gender)
        public List<int>? ProductCategoryId { get; set; }
        public List<int>? GenderCategoryId { get; set; }

        // varient level filtering
        public List<int>? ColorId { get; set; }
        public List<int>? SizeId { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // sorting (by varient Price)
        public string SortBy { get; set; } = "price"; // only price for now
        public string SortOrder { get; set; } = "asc";  // asc or desc

        // pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
