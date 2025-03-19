namespace FoodApiService.DTOs
{
    public class GetFoodQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public string? Search { get; set; }

        public int? CategoryId { get; set; }

    }
}
