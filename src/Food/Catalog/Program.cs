using Catalog.Data;
using Catalog.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Catalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? cors = builder.Configuration.GetValue<string>("Policy:cors");
            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
            string? userNameCharacters = builder.Configuration.GetValue<string>("Set:userNameCharacters");
            builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connection));

            builder.Services.AddCors(p => p.AddPolicy(cors!, builder => builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader()));
            builder.Services.AddCustomJwtAuthentication();
            builder.Services.AddIdentity<User, IdentityRole>(options => {
                options.User.AllowedUserNameCharacters = userNameCharacters!;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<DatabaseContext>();

            builder.Services
                .AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()));

            var app = builder.Build();

            app.UseCors(cors!);
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
