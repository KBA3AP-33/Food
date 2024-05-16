using Catalog.Data;
using Catalog.Models.Response;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Data;

namespace Catalog.Sources
{
    public class Index
    {
        public FilterResponse? Filters { get; set; }
        public UserStart? Users { get; set; }
        public List<StartRecipe>? Recipes { get; set; }
        public List<IngredientCount>? Ingredients { get; set; }

        public Index()
        {
            using (StreamReader reader = new StreamReader("Sources/Filters.json"))
            {
                Filters = JsonConvert.DeserializeObject<FilterResponse>(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader("Sources/Users.json"))
            {
                Users = JsonConvert.DeserializeObject<UserStart>(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader("Sources/Recipes.json"))
            {
                Recipes = JsonConvert.DeserializeObject<List<StartRecipe>>(reader.ReadToEnd());
            }

            using (StreamReader reader = new StreamReader("Sources/Ingredients.json"))
            {
                Ingredients = JsonConvert.DeserializeObject<List<IngredientCount>>(reader.ReadToEnd());
            }
        }
    }
}
