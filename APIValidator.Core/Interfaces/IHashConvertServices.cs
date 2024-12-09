#region References
using Validator.Core.CustomEntities;
using Validator.Core.Entities;
using Validator.Core.QueryFilters; 
#endregion

namespace Validator.Core.Interfaces
{
    public interface IHashConvertServices
    {
        PagedList<HashImagen> GetImagenes(QueryHashFilter filter);
        Task<HashImagen> AddImagen(HashImagen hashImagen);
        Task<HashImagen> GetImagenById(string id);
        Task<bool> DeleteImagen(string id);
    }
}