using System.ComponentModel.DataAnnotations.Schema;

namespace TesInspiro.Domain
{
    public class MsItemsViewModel
    {
        public int id { get; set; }
        public string? item_name { get; set; }
        public int? qty { get; set; }
        public int? price { get; set; }
    }
}