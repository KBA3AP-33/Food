namespace Catalog.Data
{
    public class UserRating
    {
        public int Id { get; set; }
        public Recipe? Recipe { get; set; }
        public User? User { get; set; }
        public double Rate { get; set; }
    }
}
