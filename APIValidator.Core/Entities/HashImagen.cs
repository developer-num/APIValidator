#region Refences
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations; 
#endregion

namespace Validator.Core.Entities
{
    public class HashImagen
    {
        [Required]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
       
        [Required]
        [BsonElement("firmaImagen")]
        public string? FirmaImagen { get; set; }
    }
}
