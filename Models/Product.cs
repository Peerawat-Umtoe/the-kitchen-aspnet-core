using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace the_kitchen_aspnet_core.Models
{
    public class Product
    {

        [Key]
        public int ProductId { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? Image { get; set; }

        public string? Description { get; set; }

        public int ? Price { get; set; }

        public int? Amount { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [ForeignKey("CategoryId")]
        public Category Categories { get; set; }
        public int CategoryId { get; set; }
    }
}
