using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealCheckAPI.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
