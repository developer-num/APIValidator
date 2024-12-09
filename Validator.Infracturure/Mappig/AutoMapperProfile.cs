#region References
using AutoMapper;
using Validator.Core.DTOs.InDTOs;
using Validator.Core.DTOs.OutDTOs;
using Validator.Core.Entities;
#endregion

namespace Validator.Infracturure.Mappig
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<HashImagen, InBase64Imagen>().ReverseMap();
            CreateMap<HashImagen, OutFirmaImagen>().ReverseMap();
        }
    }
}
