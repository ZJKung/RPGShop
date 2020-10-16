using System.Linq;
using System.Threading.Tasks;
using CatalogApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogApi.ViewModels;
using CatalogApi.Domain;
using Microsoft.Extensions.Options;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private const string pictureUrlTemplate = "/picture/";
        private readonly IOptionsSnapshot<CatalogSettings> _settings;
        private readonly CatalogContext _context;
        public CatalogController(CatalogContext context,IOptionsSnapshot<CatalogSettings> settings)
        {
            _settings = settings;
            _context = context;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            return Ok(await _context.CatalogTypes.ToListAsync());
        }

        [HttpGet]
        [Route("Items/{id:int}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if (id <= 0) return BadRequest();
            var item = await _context.CatalogItems.Select(x => new CatalogItemResponseVM
            {
                Id = x.Id,
                Description = x.Description,
                Price = x.Price,
                PictureUrl = $"{_settings.Value.ExternalCatalogBaseUrl}{pictureUrlTemplate}{x.PictureFileName}"
            }).FirstOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                return Ok(item);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items(int? catalogTypeId, int pageSize = 6, int pageIndex = 0)
        {
            var catalogItems = _context.CatalogItems.Where(x => catalogTypeId.HasValue ? x.CatalogTypeId == catalogTypeId : 1 == 1);

            var total = await catalogItems.LongCountAsync();

            var items = await catalogItems.Select(x => new CatalogItemResponseVM
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Price = x.Price,
                PictureUrl = $"{_settings.Value.ExternalCatalogBaseUrl}{pictureUrlTemplate}{x.PictureFileName}"
            }).OrderBy(x => x.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();
            var model = new PaginatedItemsViewModel<CatalogItemResponseVM>(pageIndex, pageSize, total, items);
            return Ok(model);
        }

        [HttpPost]
        [Route("Items")]
        public async Task<IActionResult> CreateCatalogItem([FromBody] CatalogItem catalog)
        {
            _context.CatalogItems.Add(catalog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = catalog.Id }, catalog);
        }

        [HttpPut]
        [Route("Items")]
        public async Task<IActionResult> UpdateProduct([FromBody] CatalogItem catalog)
        {
            var catalogItem = await _context.CatalogItems.FirstOrDefaultAsync(c => c.Id == catalog.Id);
            if (catalogItem == null)
            {
                return NotFound(new { Message = $"item with id {catalog.Id} not found." });
            }

            _context.CatalogItems.Update(catalog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = catalog.Id }, catalogItem);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.CatalogItems.SingleOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.CatalogItems.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}