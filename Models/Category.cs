using System.ComponentModel.DataAnnotations;

namespace the_kitchen_aspnet_core.Models
{
    public class Category
    {
        [Key]
        public int CategotyId { get; set; }

        [Required]
        public string? Name { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


    }
}
