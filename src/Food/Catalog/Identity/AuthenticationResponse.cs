using Catalog.Models.Response;

namespace Catalog.Identity
{
    public class AuthenticationResponse
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public IEnumerable<int>? Favourites { get; set; }
        public string? AccessToken { get; set; }
        public string? RefrechToken { get; set; }
        public string? Time { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
