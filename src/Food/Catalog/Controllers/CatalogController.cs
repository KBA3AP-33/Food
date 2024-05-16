using Catalog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Catalog.Models.Request;
using Catalog.Models.Response;
using Catalog.Static;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public CatalogController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        [HttpPost("recipes")]
        public IQueryable<RecipeShortResponse> Post([FromBody] List<int> recipes)
        {
            return _databaseContext.Recipes
                .Include(e => e.Ingredients)
                .Include(e => e.Author)
                .Where(e => recipes.Contains(e.Id))
                .OrderByDescending(e => e.Created)
                .Select(e => new RecipeShortResponse()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Image = e.Image,
                    Created = e.Created,
                    Rating = e.UserRatings!.Average(i => i.Rate),
                    Author = new UserResponse()
                    {
                        Id = e.Author!.Id,
                        UserName = e.Author!.UserName,
                    },
                    Status = e.Status,
                });
        }

        [HttpGet("authorize")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public CatalogResponse GetAuthorizeAdmin(int time = 1440, int start = 0, int limit = 30, string search = "", string author = "", int sort = 0)
        {
            var statuses = Request.Query["statuses"].Select(int.Parse!).ToList();
            return GetCatalogResponse(time, start, limit, search, author, sort, statuses);
        }

        [HttpGet("auth")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public CatalogResponse GetAuthorize(int time = 1440, int start = 0, int limit = 30, string search = "", string author = "", int sort = 0)
        {
            var statuses = Request.Query["statuses"]
                .Select(int.Parse!)
                .Where(e => e != 4)
                .ToList();
            return GetCatalogResponse(time, start, limit, search, author, sort, statuses);
        }

        [HttpGet]
        public CatalogResponse Get(int time = 1440, int start = 0, int limit = 30, string search = "", string author = "", int sort = 0)
        {
            return GetCatalogResponse(time, start, limit, search, author, sort);
        }

        [HttpGet("authorize/{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<RecipeResponse?> GetAuthorizeAdmin(int id)
        {
            var statuses = Request.Query["statuses"].Select(int.Parse!).ToList();
            return await GetCatalogResponse(id, statuses);
        }

        [HttpGet("auth/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<RecipeResponse?> GetAuthorize(int id)
        {
            var statuses = Request.Query["statuses"]
                .Select(int.Parse!)
                .Where(e => e != 4)
                .ToList();
            return await GetCatalogResponse(id, statuses);
        }

        [HttpGet("{id}")]
        public async Task<RecipeResponse?> Get(int id)
        {
            return await GetCatalogResponse(id);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<int> Post([FromBody] RecipeRequest recipeResponse)
        {
            var author = await _databaseContext.Users.FirstOrDefaultAsync(e => e.Id == recipeResponse.Author!.Id);
            var category = await _databaseContext.Categories.FirstOrDefaultAsync(e => e.Id == recipeResponse.CategoryId);
            var timeCategory = await _databaseContext.TimeCategories.FirstOrDefaultAsync(e => e.Id == recipeResponse.TimeCategoryId);
            var kitchen = await _databaseContext.Kitchens.FirstOrDefaultAsync(e => e.Id == recipeResponse.KitchenId);
            var status = await _databaseContext.Statuses.FirstOrDefaultAsync(e => e.Id == 2);

            var GetIngredientCount = async (RecipeIngredientRequest recipeIngredient) =>
            {
                var ingredient = await _databaseContext.Ingredients.FirstOrDefaultAsync(e => e.Name == recipeIngredient.Name);

                if (ingredient == null)
                {
                    return new IngredientCount()
                    {
                        IngredientName = recipeIngredient.Name,
                        IngredientId = null,
                        Count = (double)recipeIngredient.Count!,
                        Units = recipeIngredient.Units
                    };
                }
                return new IngredientCount()
                {
                    Ingredient = ingredient,
                    Count = (double)recipeIngredient.Count!,
                    Units = recipeIngredient.Units
                };
            };

           
            Recipe recipe = new Recipe()
            {
                Created = DateOnly.FromDateTime(DateTime.Now),
                Name = recipeResponse.Name,
                Description = recipeResponse.Description,
                Image = recipeResponse.Image,
                Author = author,
                Time = recipeResponse.Time,
                PersonCount = recipeResponse.PersonCount,
                Rating = 0.0,
                Category = category,
                Ingredients = recipeResponse.Ingredients?.Select(e => GetIngredientCount(e).Result).ToList(),
                Plan = recipeResponse?.Plan?.ToList(),
                Status = status,
                TimeCategory = timeCategory,
                Kitchen = kitchen,
            };

            var context = new ValidationContext(recipe);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(recipe, context, results, true)) return -1;

            await _databaseContext.Recipes.AddAsync(recipe);
            await _databaseContext.SaveChangesAsync();
            return recipe.Id;
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<int> Put(int id, [FromBody] RecipeRequest recipeResponse)
        {
            var recipe = await _databaseContext.Recipes.FirstOrDefaultAsync(e => e.Id == id);
            if (recipe == null) return -1;

            var category = await _databaseContext.Categories.FirstOrDefaultAsync(e => e.Id == recipeResponse.CategoryId);
            var timeCategory = await _databaseContext.TimeCategories.FirstOrDefaultAsync(e => e.Id == recipeResponse.TimeCategoryId);
            var kitchen = await _databaseContext.Kitchens.FirstOrDefaultAsync(e => e.Id == recipeResponse.KitchenId);
            var status = await _databaseContext.Statuses.FirstOrDefaultAsync(e => e.Id == recipeResponse.StatusId);

            var GetIngredientCount = async (RecipeIngredientRequest recipeIngredient) =>
            {
                var ingredient = await _databaseContext.Ingredients.FirstOrDefaultAsync(e => e.Name == recipeIngredient.Name);

                if (ingredient == null)
                {
                    return new IngredientCount()
                    {
                        IngredientName = recipeIngredient.Name,
                        IngredientId = null,
                        Count = (double)recipeIngredient.Count!,
                        Units = recipeIngredient.Units
                    };
                }
                return new IngredientCount()
                {
                    Ingredient = ingredient,
                    Count = (double)recipeIngredient.Count!,
                    Units = recipeIngredient.Units
                };
            };
            _databaseContext.IngredientCounts.RemoveRange(_databaseContext.IngredientCounts.Where(e => e.RecipeId == id));

            recipe.Name = recipeResponse.Name;
            recipe.Description = recipeResponse.Description;
            recipe.Image = recipeResponse.Image;
            recipe.Time = recipeResponse.Time;
            recipe.PersonCount = recipeResponse.PersonCount;
            recipe.Category = category;
            recipe.TimeCategory = timeCategory;
            recipe.Ingredients = recipeResponse.Ingredients?.Select(e => GetIngredientCount(e).Result).ToList();
            recipe.Kitchen = kitchen;
            recipe.Plan = recipeResponse.Plan;
            recipe.Status = status;      

            return await _databaseContext.SaveChangesAsync();
        }

        [HttpPut("status/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<int> Put(int id, [FromBody] RecipeStatusRequest recipeStatusRequest)
        {
            var recipe = await _databaseContext.Recipes.FirstOrDefaultAsync(e => e.Id == id);
            if (recipe == null) return -1;

            var status = await _databaseContext.Statuses.FirstOrDefaultAsync(e => e.Id == recipeStatusRequest.StatusId);
            recipe.Status = status;

            await _databaseContext.SaveChangesAsync();
            return id;
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<int> Delete(int id)
        {
            var recipe = await _databaseContext.Recipes.FirstOrDefaultAsync(e => e.Id == id);
            if (recipe == null) return -1;

            _databaseContext.Recipes.Remove(recipe);
            await _databaseContext.SaveChangesAsync();
            return id;
        }

        private CatalogResponse GetCatalogResponse(int time, int start, int limit, string search, string author, int sort, List<int>? statuses = null)
        {
            var categories = Request.Query["categories"].Select(int.Parse!).ToList();
            var ingredients = Request.Query["ingredients"].Select(int.Parse!).ToList();
            var kitchens = Request.Query["kitchens"].Select(int.Parse!).ToList();
            var times = Request.Query["times"].Select(int.Parse!).ToList();
            var tags = search.Split(" ").Select(e => e.ToLower()).ToList();
            var order = sort switch
            {
                0 => new { Name = "Rating", isDescending = true },
                1 => new { Name = "Created", isDescending = true },
                _ => new { Name = "Created", isDescending = false },
            };
            time = time > 1440 ? 1440 : time;
            limit = limit > 30 ? 30 : limit;

            Expression<Func<Recipe, bool>> searchPredicate = (item) => tags.Count == 0;
            Expression<Func<Recipe, bool>> ingredientPredicate = (item) => ingredients.Count == 0;
            Expression<Func<Recipe, bool>> categoryPredicate = (item) => categories.Count == 0;
            Expression<Func<Recipe, bool>> kitchenPredicate = (item) => kitchens.Count == 0;
            Expression<Func<Recipe, bool>> timesPredicate = (item) => times.Count == 0;
            Expression<Func<Recipe, bool>> statusPredicate = (item) => (statuses == null || statuses.Count == 0) && item.Status!.Name == "Одобрено";

            tags.ForEach(e => searchPredicate = searchPredicate.OrElse((item) => item.Name!.ToLower().Contains(e)));
            ingredients.ForEach(e => ingredientPredicate = ingredientPredicate.OrElse((item) => item.Ingredients!.Select(i => i.Ingredient!.Id).Contains(e)));
            categories.ForEach(e => categoryPredicate = categoryPredicate.OrElse((item) => categories.Contains(item.Category!.Id)));
            kitchens.ForEach(e => kitchenPredicate = kitchenPredicate.OrElse((item) => kitchens.Contains(item.Kitchen!.Id)));
            times.ForEach(e => timesPredicate = timesPredicate.OrElse((item) => times.Contains(item.TimeCategory!.Id)));
            statuses?.ForEach(e => statusPredicate = statusPredicate.OrElse((item) => statuses.Contains(item.Status!.Id)));

            var result = _databaseContext.Recipes
                .Include(e => e.Ingredients)
                .Include(e => e.Author)
                .Where(searchPredicate
                    .AndAlso(ingredientPredicate)
                    .AndAlso(categoryPredicate)
                    .AndAlso(kitchenPredicate)
                    .AndAlso(timesPredicate)
                    .AndAlso(statusPredicate)
                    .AndAlso(e => e.Time <= time && e.Author!.UserName!.ToLower().Contains(author.ToLower())))
                .DynamicOrderBy(order.Name, order.isDescending);

            var recipes = result
                .Skip(start)
                .Take(limit)
                .Select(e => new RecipeShortResponse()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Image = e.Image,
                    Created = e.Created,
                    Rating = e.Rating,
                    Author = new UserResponse()
                    {
                        Id = e.Author!.Id,
                        UserName = e.Author!.UserName,
                        Image = e.Author!.Image,
                    },
                    Status = e.Status,
                });

            return new CatalogResponse()
            {
                Start = start,
                Limit = limit,
                Recipes = recipes,
                Count = result.Count(),
            };
        }

        private async Task<RecipeResponse?> GetCatalogResponse(int id, List<int>? statuses = null)
        {
            Recipe? recipe = await _databaseContext.Recipes
                .Include(e => e.Author)
                .Include(e => e.Ingredients!)
                .ThenInclude(e => e.Ingredient)
                .Include(e => e.Kitchen)
                .Include(e => e.Category)
                .Include(e => e.TimeCategory)
                .Include(e => e.Status)
                .Include(e => e.UserRatings!)
                .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id && ((statuses == null || statuses.Count == 0) && e.Status!.Id == 1 || (statuses != null && statuses.Contains(e.Status!.Id))));

            if (recipe == null) return null;

            return new RecipeResponse()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                Created = recipe.Created,
                Image = recipe.Image,
                Time = recipe.Time,
                PersonCount = recipe.PersonCount,
                Rating = recipe.Rating,
                Author = new UserResponse()
                {
                    Id = recipe.Author?.Id,
                    UserName = recipe.Author?.UserName,
                    Image = recipe.Author?.Image,
                },
                Ingredients = recipe.Ingredients?.Select(e => new IngredientCountResponse()
                {
                    Id = e.Id,
                    IngredientName = e.IngredientName,
                    Ingredient = e.Ingredient, 
                    Count = e.Count,
                    Units = e.Units
                }),
                Plan = recipe.Plan,
                Status = recipe.Status,
                Kitchen = recipe.Kitchen,
                Category = recipe.Category,
                TimeCategory = recipe.TimeCategory,
                UserRatings = recipe.UserRatings?.Select(e => new UserRatingResponse() { UserId = e.User!.Id, Rate = e.Rate }),
            };
        }
    }
}
