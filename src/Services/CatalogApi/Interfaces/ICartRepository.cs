using System.Collections.Generic;
using CatalogApi.Domain;

namespace CatalogApi.Interfaces
{
    public interface ICartRepository:IGenericRepository<Cart>
    {
         IEnumerable<string> GetUsers();
    }
}