using System.Text.Json.Serialization;

namespace FoodApiService.Entities
{
    public  sealed class Food : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        [JsonIgnore]
        public ICollection<FoodImage> FoodImages { get; set; } = [];
    }
}
