using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ILogger<CatalogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetCatalog()
        {
            var catalog = new CatalogItem[]
            {
                new CatalogItem { Id = new Guid("11111111-0000-0000-0000-000000000000"), Name = "AMD Ryzen 3 2200G", UnitPrice = 11.11m },
                new CatalogItem { Id = new Guid("22222222-0000-0000-0000-000000000000"), Name = "Intel i3 8400", UnitPrice = 22.22m },
                new CatalogItem { Id = new Guid("33333333-0000-0000-0000-000000000000"), Name = "AMD Ryzen 5 3600", UnitPrice = 33.33m },
                new CatalogItem { Id = new Guid("44444444-0000-0000-0000-000000000000"), Name = "Intel i7 10700K", UnitPrice = 44.44m },
                new CatalogItem { Id = new Guid("55555555-0000-0000-0000-000000000000"), Name = "AMD Ryzen 9 5900X", UnitPrice = 55.55m },
            };

            return Ok(catalog);
        }
    }

    public class CatalogItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }
}