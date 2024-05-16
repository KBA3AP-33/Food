using Catalog.Data;

namespace Catalog.Sources
{
    public class StartRecipe
    {
        public int Id { get; set; }
        public DateOnly Created { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public User? Author { get; set; }
        public string? AuthorId { get; set; }
        public int Time { get; set; }
        public int PersonCount { get; set; }
        public double Rating { get; set; }
        public Kitchen? Kitchen { get; set; }
        public int? KitchenId { get; set; } = new();
        public Category? Category { get; set; }
        public int? CategoryId { get; set; } = new();
        public TimeCategory? TimeCategory { get; set; }
        public int? TimeCategoryId { get; set; } = new();
        public List<IngredientCount>? Ingredients { get; set; }
        public List<string>? Plan { get; set; }
        public Status? Status { get; set; }
        public int? StatusId { get; set; } = new();
        public string? Message { get; set; }
        public List<User>? UsersLikes { get; set; } = new();
        public List<UserRating>? UserRatings { get; set; } = new();
    }
}
