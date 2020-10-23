using System.Text.Json.Serialization;

namespace WebMVC.Models
{
    public class CatalogItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]

        public string Name { get; set; }
        [JsonPropertyName("description")]

        public string Description { get; set; }
        [JsonPropertyName("price")]

        public decimal Price { get; set; }
        [JsonPropertyName("pictureUrl")]

        public string PictureUrl { get; set; }
    }
}