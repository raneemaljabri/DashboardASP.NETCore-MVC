using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]

        public string Description { get; set; }












    }
}
