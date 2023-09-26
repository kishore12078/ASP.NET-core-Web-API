using System.ComponentModel.DataAnnotations;

namespace CityInfoAPI.Entities
{
    public class PointOfInterestForCreationDTO
    {
        [Required(ErrorMessage ="You Should Provide Name of the City")]
        [MaxLength(20,ErrorMessage ="Name should be Maximum of 20 characters")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200,ErrorMessage ="Description should be maximum of 200 Characters")]
        public string Description { get; set; } = string.Empty;
    }
}
