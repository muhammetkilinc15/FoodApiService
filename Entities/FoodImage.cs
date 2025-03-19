using System.Text.Json.Serialization;

namespace FoodApiService.Entities
{
    public sealed class FoodImage : BaseEntity
    {
        public int FoodId { get; set; }
        [JsonIgnore]
        public Food Food { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
    }
}
