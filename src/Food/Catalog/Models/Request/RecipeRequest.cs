using Catalog.Data;
using Catalog.Models.Response;

namespace Catalog.Models.Request
{
    public class RecipeRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public UserResponse? Author { get; set; }
        public int Time { get; set; }
        public int PersonCount { get; set; }
        public int CategoryId { get; set; }
        public int TimeCategoryId { get; set; }
        public int KitchenId { get; set; }
        public int StatusId { get; set; }
        public List<RecipeIngredientRequest>? Ingredients { get; set; }
        public List<string>? Plan { get; set; }
        public string? Message { get; set; }
    }
}
