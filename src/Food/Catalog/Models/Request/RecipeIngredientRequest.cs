namespace Catalog.Models.Request
{
    public class RecipeIngredientRequest
    {
        public string? Name { get; set; }
        public double? Count { get; set; }
        public string? Units { get; set; }
    }
}
