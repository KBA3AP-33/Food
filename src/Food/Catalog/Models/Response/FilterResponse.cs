using Catalog.Data;


namespace Catalog.Models.Response
{
    public class FilterResponse
    {
        public IEnumerable<Kitchen>? Kitchens { get; set; }
        public IEnumerable<TimeCategory>? TimeCategories { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Ingredient>? Ingredients { get; set; }
        public IEnumerable<Status>? Statuses { get; set; }
    }
}
