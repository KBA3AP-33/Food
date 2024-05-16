using Microsoft.AspNetCore.Identity;

namespace Catalog.Data
{
    public class User : IdentityUser
    {
        public string? Image { get; set; }
        public List<Recipe>? FavouriteRecipes { get; set; }
        public List<Recipe>? Recipes { get; set; }
        public string? RefreshToken { get; set; }
        public DateOnly RefreshTokenExpiryTime { get; set; }
        public DateOnly Created { get; set; }
    }
}
