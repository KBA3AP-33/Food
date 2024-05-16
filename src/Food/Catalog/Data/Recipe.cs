using System.ComponentModel.DataAnnotations;

namespace Catalog.Data
{
    public class Recipe
    {
        public int Id { get; set; }
        public DateOnly Created { get; set; }

        [Required]
        [MinLength(5), MaxLength(100)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public User? Author { get; set; }

        [Required]
        [Range(0, 1440)]
        public int Time { get; set; }

        [Required]
        [Range(0, 10)]
        public int PersonCount { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }
        public Kitchen? Kitchen { get; set; }
        public Category? Category { get; set; }
        public TimeCategory? TimeCategory { get; set; }
        public List<IngredientCount>? Ingredients { get; set; }
        public List<string>? Plan { get; set; }
        public Status? Status { get; set; }
        public List<User>? UsersLikes { get; set; }
        public List<UserRating>? UserRatings { get; set; }
    }
}
