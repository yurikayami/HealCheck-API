using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealCheckAPI.Models
{
    [Table("Images")]
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Path { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Kcal { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Gam { get; set; }

        [MaxLength(255)]
        public string? FoodName { get; set; }

        public string? AiSuggestion { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public ICollection<Analysis> Analyses { get; set; } = new List<Analysis>();
    }
}
