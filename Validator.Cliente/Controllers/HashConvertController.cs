#region Refences
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using Validator.Cliente.Responses;
using Validator.Core.CustomEntities;
using Validator.Core.DTOs.InDTOs;
using Validator.Core.DTOs.OutDTOs;
using Validator.Core.Entities;
using Validator.Core.Interfaces;
using Validator.Core.QueryFilters;
using Validator.Infracturure.Interfaces;
using Validator.Infracturure.Tool.Security;
#endregion

namespace Validator.Cliente.Controllers
{
    #region Decorator
    [ApiKeyAuth]    
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    #endregion

    public class HashConvertController : ControllerBase
    {
        #region DependecyInjection
        private readonly IHashConvertServices _hashConvertServices;
        private readonly IMapper _mapper;
        private readonly ILogger<HashConvertController> _logger;
        private readonly ISHA1 _sHA1;
        private readonly ISHA256 _sHA256;
        private readonly ISHA384 _sHA384;
        private readonly ISHA512 _sHA512;
        public HashConvertController(IHashConvertServices hashConvertServices, IMapper mapper, ILogger<HashConvertController> logger,
            ISHA1 sHA1, ISHA256 sHA256, ISHA384 sHA384, ISHA512 sHA512)
        {
            _hashConvertServices = hashConvertServices;
            _mapper = mapper;
            _logger = logger;
            _sHA1 = sHA1;
            _sHA256 = sHA256;
            _sHA384 = sHA384;
            _sHA512 = sHA512;
        }
        #endregion

        #region GetAlls
        /// <summary>
        /// Get alls products
        /// </summary>
        /// <param name="ApiKey"></param>
        /// <param name="filter"></param>
        /// <returns>return list</returns>
        [ActionName(nameof(GetAllsHash))]
        [HttpGet(template: "", Name = nameof(GetAllsHash))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<OutFirmaImagen>>))]
        public IActionResult GetAllsHash([FromHeader] string ApiKey, [FromQuery] QueryHashFilter filter)
        {
            try
            {
                Stopwatch time = Time();
                var imagenes = _hashConvertServices.GetImagenes(filter);
                var imagenesDTO = _mapper.Map<IEnumerable<OutFirmaImagen>>(imagenes);
                var linkBuilder = new PageLinkBuilder(Url, "GetAllsHash", null, imagenes.TotalPages, imagenes.PageSize, imagenes.TotalPages);

                var paging = new
                {
                    First = linkBuilder.FirstPage,
                    Previous = linkBuilder.PreviousPage,
                    Next = linkBuilder.NextPage,
                    Last = linkBuilder.LastPage
                };

                var metadata = new MetaData
                {
                    TotalCount = imagenes.TotalCount,
                    PageSize = imagenes.PageSize,
                    CurrentPage = imagenes.CurrentPage,
                    TotalPages = imagenes.TotalPages,
                    HasNextPage = imagenes.HasNext,
                    HasPreviousPage = imagenes.HasPrevious,
                };

                var response = new ApiResponse<IEnumerable<OutFirmaImagen>>(imagenesDTO)
                {
                    Meta = metadata,
                    Status = Convert.ToString((int)HttpStatusCode.OK),
                    Message = $"Exitoso"
                };

                Response.Headers.Add("X-RequestTime", Convert.ToString(time.Elapsed));
                Response.Headers.Add("X-Fist", Convert.ToString(paging.First));
                Response.Headers.Add("X-Previus", Convert.ToString(paging.Previous));
                Response.Headers.Add("X-Next", Convert.ToString(paging.Next));
                Response.Headers.Add("X-Last", Convert.ToString(paging.Last));
                time.Stop();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ResponseException(ex);
            }
        }
        #endregion

        #region Get
        /// <summary>
        /// Insert the id of the image to compare
        /// </summary>
        /// <param name="ApiKey"></param>
        /// <param name="id"></param>
        /// <returns>Return Hash</returns>
        [ActionName(nameof(GetHashById))]
        [HttpGet(template: "{id}", Name = nameof(GetHashById))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<OutFirmaImagen>))]
        public async Task<IActionResult> GetHashById([FromHeader] string ApiKey, string id)
        {
            try
            {
                Stopwatch time = Time();
                var imagen = await _hashConvertServices.GetImagenById(id);
                if(imagen == null)
                    return NotFound("Id no existe.");
                var imagenDTO = _mapper.Map<OutFirmaImagen>(imagen);
                var response = new ApiResponse<OutFirmaImagen>(imagenDTO);
                Response.Headers.Add("X-RequestTime", Convert.ToString(time.Elapsed));
                Response.Headers.Add("X-StringLength", Convert.ToString(imagen.FirmaImagen?.Length));
                var num = imagen.FirmaImagen?.Length;
                var TypeEncrypt = num switch
                {
                    (28) => "SHA1",
                    (64) => "SHA256",
                    (96) => "SHA384",
                    (128) => "SHA512",
                    _ => throw new Exception("Error")
                };
                Response.Headers.Add("X-TypeEncrypt", TypeEncrypt);
                time.Stop();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ResponseException(ex);
            }
        } 
        #endregion

        #region Post
        /// <summary>
        /// Send the image in base64 to get a hash
        /// </summary>
        /// <param name="ApiKey"></param>
        /// <param name="TypeEncrypt">Hash type to use ( _ o Default is SHA1, SHA256, SHA384 and SHA512)</param>
        /// <param name="base64"></param>
        /// <returns>Return hash</returns>
        [ActionName(nameof(PostImagen))]
        [HttpPost(template: "", Name = nameof(PostImagen))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OutFirmaImagen))]
        public async Task<IActionResult> PostImagen([FromHeader] string ApiKey, string TypeEncrypt, [FromBody] InBase64Imagen base64)
        {
            try
            {
                if (TypeEncrypt.ToUpper() == "_" || TypeEncrypt.ToUpper() == "Defauld")
                    TypeEncrypt = "SHA1";

                if (base64 != null && base64.FirmaImagen.Length != 0)
                {
                    Stopwatch time = Time();
                    base64.FirmaImagen = (TypeEncrypt.ToUpper()) switch
                    {
                        ("SHA1") => _sHA1.GetSHA1(base64.FirmaImagen),
                        ("SHA256") => _sHA256.GetSHA256(base64.FirmaImagen),
                        ("SHA384") => _sHA384.GetSHA384(base64.FirmaImagen),
                        ("SHA512") => _sHA512.GetSHA512(base64.FirmaImagen),
                        _ => throw new Exception("Opcion no valida.")
                    };

                    var imagen = _mapper.Map<HashImagen>(base64);
                    await _hashConvertServices.AddImagen(imagen);
                    var imagenDto = _mapper.Map<OutFirmaImagen>(imagen);
                    var response = new ApiResponse<OutFirmaImagen>(imagenDto);
                    Response.Headers.Add("X-RequestTime", Convert.ToString(time.Elapsed));
                    Response.Headers.Add("X-StringLength", Convert.ToString(imagenDto.FirmaImagen.Length));
                    Response.Headers.Add("X-TypeEncrypt", TypeEncrypt);
                    time.Stop();
                    return Ok(response);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return ResponseException(ex);
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete hash, only system function after one year
        /// </summary>
        /// <param name="ApiKey"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName(nameof(DeleteHash))]
        [HttpDelete(template: "{id}", Name = nameof(DeleteHash))]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> DeleteHash([FromHeader] string ApiKey, string id)
        {
            try
            {
                Stopwatch time = Time();
                var result = await _hashConvertServices.DeleteImagen(id);
                var response = new ApiResponse<bool>(result);
                time.Stop();
                return Ok(response);

            }
            catch (Exception ex)
            {
                return ResponseException(ex);
            }
        } 
        #endregion

        #region Time
        private static Stopwatch Time()
        {
            var time = new Stopwatch();
            time.Start();
            return time;
        }
        #endregion

        #region Exception
        private IActionResult Exception(Exception ex)
        {
            var json = new Error
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Title = "Internal server error",
                Detail = ex.StackTrace
            };

            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseError<object>(ex.Message)
            {
                DetailError = json
            });
        }
        #endregion

        #region ResponseException
        private IActionResult ResponseException(Exception ex)
        {
            Stopwatch time = Time();
            _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
            Response.Headers.Add("X-RequestTime", Convert.ToString(time.Elapsed));
            time.Stop();
            return Exception(ex);
        }
        #endregion
    }
}
