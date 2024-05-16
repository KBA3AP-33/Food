namespace Catalog.Models.Request
{
    public class RecipeFavouriteRequest
    {
        public string? UserId { get; set; }
        public int RecipeId { get; set; }
    }
}
