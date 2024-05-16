using Catalog.Data;

namespace Catalog.Models.Response
{
    public class RecipeResponse
    {
        public int Id { get; set; }
        public DateOnly Created { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public UserResponse? Author { get; set; }
        public int Time { get; set; }
        public int PersonCount { get; set; }
        public double Rating { get; set; }
        public IEnumerable<IngredientCountResponse>? Ingredients { get; set; }
        public List<string>? Plan { get; set; }
        public Status? Status { get; set; }
        public Kitchen? Kitchen { get; set; }
        public Category? Category { get; set; }
        public TimeCategory? TimeCategory { get; set; }
        public IEnumerable<UserRatingResponse>? UserRatings { get; set; }
    }
}
