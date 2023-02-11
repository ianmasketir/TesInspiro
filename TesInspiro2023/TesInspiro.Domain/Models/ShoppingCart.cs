using System.ComponentModel.DataAnnotations.Schema;

namespace TesInspiro.Domain
{
    [Table("shopping_cart")]
    public class ShoppingCart
    {
        public int? id { get; set; }
        public int? item_id { get; set; }
        public string? username { get; set; }
        public int? qty { get; set; }
        public int? total_price { get; set; }
        public bool? is_paid { get; set; }
        public DateTime? created_dtm { get; set; }
        public DateTime? updated_dtm { get; set; }
    }
}