using System.ComponentModel.DataAnnotations;

namespace BulkyBooks.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
