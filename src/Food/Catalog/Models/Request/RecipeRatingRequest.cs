namespace Catalog.Models.Request
{
    public class RecipeRatingRequest
    {
        public string? UserId { get; set; }
        public int RecipeId { get; set; }
        public double Rate { get; set; }
    }
}
