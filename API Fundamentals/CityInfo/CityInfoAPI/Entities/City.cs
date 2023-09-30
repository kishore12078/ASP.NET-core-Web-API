using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfoAPI.Entities
{
    public class City
    {
        public City(string name)
        {
            Name = name;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Description { get; set; } = string.Empty;
        public List<PointOfInterest> PointsOfInterests { get; set; } = new List<PointOfInterest>();
    }
}
