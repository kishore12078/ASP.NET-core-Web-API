using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfoAPI.Entities
{
    public class PointOfInterest
    {
        public PointOfInterest(string name)
        {
            Name = name;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public City? City { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
