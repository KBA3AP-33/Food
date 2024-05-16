using Catalog.Data;
using Catalog.Models.Request;
using Catalog.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public UsersController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet("{id}")]
        public async Task<UserInfoResponse?> Get(string id)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            return (user == null) ? null : new UserInfoResponse() { Image = user.Image };
        }

        [HttpPost("favourite")]
        public async Task<int?> Post([FromBody] RecipeFavouriteRequest request)
        {
            var user = await _databaseContext.Users.Include(e => e.FavouriteRecipes).FirstOrDefaultAsync(e => e.Id == request.UserId);
            if (user == null) return null;

            var recipe = await _databaseContext.Recipes.FirstOrDefaultAsync(e => e.Id == request.RecipeId);
            if (recipe == null) return null;

            int result = -1;
            if (user.FavouriteRecipes!.Contains(recipe))
            {
                user.FavouriteRecipes.Remove(recipe);
                result = request.RecipeId;
            }
            else if (user.FavouriteRecipes!.Count < 50)
            {
                user.FavouriteRecipes.Add(recipe);
                result = request.RecipeId;
            }

            await _databaseContext.SaveChangesAsync();
            return result;
        }

        [HttpPost("rating")]
        public async Task<double?> Post([FromBody] RecipeRatingRequest request)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(e => e.Id == request.UserId);
            if (user == null) return null;

            var recipe = await _databaseContext.Recipes.Include(e => e.UserRatings).FirstOrDefaultAsync(e => e.Id == request.RecipeId);
            if (recipe == null) return null;
            if (recipe.UserRatings!.Select(e => e.User?.Id).Contains(user.Id)) return null;

            recipe.UserRatings?.Add(new UserRating() { Recipe = recipe, User = user, Rate = request.Rate });
            recipe.Rating = recipe.UserRatings!.Average(e => e.Rate);
            await _databaseContext.SaveChangesAsync();
            return recipe.Rating;
        }

        [HttpPut("{id}")]
        public async Task<UserInfoResponse?> Put(string id, [FromBody] UserInfoRequest request)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return null;

            user.Image = request.Image;
            await _databaseContext.SaveChangesAsync();
            return new UserInfoResponse() { Image = user.Image };      
        }
    }
}
