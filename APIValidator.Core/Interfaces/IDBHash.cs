#region References
using MongoDB.Driver;
using Validator.Core.Entities;

#endregion
namespace Validator.Core.Interfaces
{
    public interface IDBHash
    {
        IMongoCollection<HashImagen> GetHashImagensCollection();
        IMongoCollection<InfoImagen> GetProductsCollectionQr();
    }
}