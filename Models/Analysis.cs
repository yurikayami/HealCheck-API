using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealCheckAPI.Models
{
    [Table("Analysis")]
    public class Analysis
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ImageId { get; set; }

        [Required]
        public int NutrientId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Value { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Confidence { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("ImageId")]
        public Image Image { get; set; } = null!;

        [ForeignKey("NutrientId")]
        public Nutrient Nutrient { get; set; } = null!;
    }
}
