using Catalog.Data;

namespace Catalog.Models.Response
{
    public class IngredientCountResponse
    {
        public int? Id { get; set; }
        public string? IngredientName { get; set; }
        public Ingredient? Ingredient { get; set; }
        public double Count { get; set; }
        public string? Units { get; set; }
    }
}
