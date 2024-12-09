#region References
using MongoDB.Driver;
using Validator.Core.CustomEntities;
using Validator.Core.Entities;
using Validator.Core.Interfaces;
using Validator.Core.QueryFilters;
#endregion

namespace Validator.Core.Services
{
    public class HashConvertServices : IHashConvertServices
    {
        #region DependencyInjection
        private readonly IMongoCollection<HashImagen> _hashImagen;
        public HashConvertServices(IDBHash dBHash)
        {
            _hashImagen = dBHash.GetHashImagensCollection();
        }
        #endregion

        #region GetAllsImagen
        public PagedList<HashImagen> GetImagenes(QueryHashFilter filter)
        {
            var imagenes = _hashImagen.Find(imagenes => true).ToEnumerable();
            PagedList<HashImagen> PageImagenes = PagedList<HashImagen>.ToPagedList(imagenes.AsQueryable(), filter.PageNumber, filter.PageSize);
            return PageImagenes;
        }
        #endregion

        #region GetByIdImagen
        public async Task<HashImagen> GetImagenById(string id)
        {
            var imagen = await _hashImagen.Find(x => x.Id == id).FirstOrDefaultAsync();
            return imagen;
        }
        #endregion

        #region PostImagen
        public async Task<HashImagen> AddImagen(HashImagen hashImagen)
        {
            await _hashImagen.InsertOneAsync(hashImagen);
            return hashImagen;
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteImagen(string id)
        {
            await _hashImagen.DeleteOneAsync(x => x.Id == id);
            return true;
        }
        #endregion
    }
}
