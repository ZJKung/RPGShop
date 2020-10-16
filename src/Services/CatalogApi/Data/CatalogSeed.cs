using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Data
{
    public class CatalogSeed
    {
        public static async Task SeedAsync(CatalogContext context)
        {
            if (!context.CatalogTypes.Any())
            {
                context.CatalogTypes.AddRange();
                await context.SaveChangesAsync();
            }
        }
    }
}