namespace CityInfoAPI.Entities
{
    public class City
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
        public List<PointsOfInterest> PointsOfInterests { get; set; }=new List<PointsOfInterest>();
    }
}
