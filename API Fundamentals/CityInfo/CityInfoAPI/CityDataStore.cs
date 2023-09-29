using CityInfoAPI.Entities;

namespace CityInfoAPI
{
    public class CityDataStore
    {
        public List<CityDTO> Cities { get; set; }
        //public static CityDataStore Current { get;}=new CityDataStore(); //there is no need to create object, with these we can able to access methods like instance
        public CityDataStore() 
        {
            Cities = new List<CityDTO>() {
                new CityDTO(){ 
                    Id=1,Name="Chennai",Description="Wonder ful place to live with cheap",
                    PointsOfInterests=new List<PointsOfInterestDTO> { 
                        new PointsOfInterestDTO{ Id=1,Name="Kishore",Description="Learninig"},
                        new PointsOfInterestDTO{ Id=2,Name="Mathan",Description="Learninig"}
                    }
                },
                new CityDTO(){ 
                    Id=2,Name="Mumbai",Description="Wonder ful place for tourism but congested",
                     PointsOfInterests=new List<PointsOfInterestDTO> {
                        new PointsOfInterestDTO{ Id=1,Name="Gokul",Description="Learninig"},
                        new PointsOfInterestDTO{ Id=2,Name="Kombaiya",Description="Learninig"}
                    }
                },
                new CityDTO(){ Id=3,Name="Cochin",Description="Pleasant tourist place in india"}
            };
        }
    }
}
