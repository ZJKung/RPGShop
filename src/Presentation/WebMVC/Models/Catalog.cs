using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebMVC.Models
{
    public class Catalog
    {
        [JsonPropertyName("pageIndex")]
        public int PageIndex { get; set; }
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("data")]
        public List<CatalogItem> Data { get; set; }
    }
}