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
        public int Id { get; set; }
        [Required]
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public City? City { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
