namespace FoodApiService.DTOs
{
    public sealed record UpdateFoodDto
    {
        public int Id { get; set; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int CategoryId { get; init; }
    }
}
