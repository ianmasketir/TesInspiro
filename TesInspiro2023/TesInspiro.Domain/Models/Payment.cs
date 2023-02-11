using System.ComponentModel.DataAnnotations.Schema;

namespace TesInspiro.Domain
{
    [Table("payment")]
    public class Payment
    {
        public int id { get; set; }
        public string? username { get; set; }
        public int? grand_total { get; set; }
        public int? pay { get; set; }
        public DateTime? created_dtm { get; set; }
    }
}