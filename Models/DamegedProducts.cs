using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class DamegedProducts
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Qty { get; set; }
    }
}