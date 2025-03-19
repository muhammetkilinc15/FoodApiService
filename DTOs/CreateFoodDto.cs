namespace FoodApiService.DTOs
{
    public sealed record CreateFoodDto
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int CategoryId { get; init; }
        public IFormFileCollection Images { get; init; }

    }
}
