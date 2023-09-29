namespace CityInfoAPI.Entities
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CityPoints
        {
            get
            {
                return PointsOfInterests.Count();
            }
        }
        public List<PointsOfInterestDTO> PointsOfInterests { get; set; }=new List<PointsOfInterestDTO>();
    }
}
