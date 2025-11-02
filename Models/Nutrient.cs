using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealCheckAPI.Models
{
    [Table("Nutrients")]
    public class Nutrient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Unit { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation property
        public ICollection<Analysis> Analyses { get; set; } = new List<Analysis>();
    }
}
