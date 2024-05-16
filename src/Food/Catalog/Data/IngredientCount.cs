namespace Catalog.Data
{
    public class IngredientCount
    {
        public int? Id { get; set; }
        public string? IngredientName { get; set; }
        public Ingredient? Ingredient { get; set; }
        public int? IngredientId { get; set; } = new();
        public double Count { get; set; }
        public string? Units { get; set; }
        public Recipe? Recipe { get; set; }
        public int? RecipeId { get; set; } = new();
    }
}
