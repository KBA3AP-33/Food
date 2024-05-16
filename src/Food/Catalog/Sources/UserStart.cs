using Catalog.Data;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Catalog.Sources
{
    public class UserStart
    {
        public List<User>? Users { get; set; }
        public List<IdentityRole>? Roles { get; set; }
        public List<IdentityUserRole<string>>? UserRoles { get; set; }
    }
}
