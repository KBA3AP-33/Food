using Catalog.Data;
using Catalog.Models.Response;
using Microsoft.AspNetCore.Mvc;


namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiltersController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public FiltersController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public FilterResponse Get()
        {
            return new FilterResponse()
            {
                Kitchens = _databaseContext.Kitchens,
                TimeCategories = _databaseContext.TimeCategories,
                Categories = _databaseContext.Categories,
                Ingredients = _databaseContext.Ingredients,
                Statuses = _databaseContext.Statuses,
            };
        }
    }
}
