using Catalog.Data;

namespace Catalog.Models.Response
{
    public class RecipeShortResponse
    {
        public int Id { get; set; }
        public DateOnly Created { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public double? Rating { get; set; }
        public UserResponse? Author { get; set; }
        public Status? Status { get; set; }
    }
}
