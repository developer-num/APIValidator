#region References
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Validator.Core.Entities;
using Validator.Core.Interfaces;
using Validator.Infracturure.ConfigurationDB;
#endregion

namespace Validator.Infracturure.Repositories
{
    public class DBHash : IDBHash
    {
        private readonly IMongoCollection<HashImagen> _hashImagen;
        private readonly IMongoCollection<InfoImagen> _infoImagen;
        public DBHash(IOptions<ConfigDBValidatorImege> configDbValidator)
        {
            var client = new MongoClient(configDbValidator.Value.Connection_String);
            var database = client.GetDatabase(configDbValidator.Value.DataBase_Name);
            _hashImagen = database.GetCollection<HashImagen>(configDbValidator.Value.Collection_Data);
            _infoImagen = database.GetCollection<InfoImagen>(configDbValidator.Value.Collection_Info);
        }
        public IMongoCollection<HashImagen> GetHashImagensCollection() => _hashImagen;
        public IMongoCollection<InfoImagen> GetProductsCollectionQr() => _infoImagen;
    }
}
