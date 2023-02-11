using System.ComponentModel.DataAnnotations.Schema;

namespace TesInspiro.Domain
{
    public class ShoppingCartViewModel
    {
        public int? id { get; set; }
        public string? item_name { get; set; }
        public string? username { get; set; }
        public int? qty { get; set; }
        public int? price { get; set; }
        public int? total_price { get; set; }
        public bool? is_paid { get; set; }
        public DateTime? created_dtm { get; set; }
        public DateTime? updated_dtm { get; set; }
    }
    public class ShoppingCart_DTO
    {
        public int? id { get; set; }
        public int? item_id { get; set; }
        public string? username { get; set; }
        public int? qty { get; set; }
        public int? price { get; set; }
        public DateTime? updated_dtm { get; set; }
    }
}