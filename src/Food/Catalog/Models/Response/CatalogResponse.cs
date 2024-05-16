namespace Catalog.Models.Response
{
    public class CatalogResponse
    {
        public int Start { get; set; }
        public int Limit { get; set; }
        public IEnumerable<RecipeShortResponse>? Recipes { get; set; }
        public int Count { get; set; }
    }
}
