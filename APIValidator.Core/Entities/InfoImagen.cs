#region References
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations; 
#endregion

namespace Validator.Core.Entities
{
    public class InfoImagen
    {
        [Required]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [BsonElement("idImagen")]
        public string? IdImagen { get; set; }

        [Required]
        [BsonElement("base64Imagen")]
        public string? Base64Imagen { get; set; }

        [Required]
        [BsonElement("date")]
        public DateTime Date { get; set; }
    }
}
