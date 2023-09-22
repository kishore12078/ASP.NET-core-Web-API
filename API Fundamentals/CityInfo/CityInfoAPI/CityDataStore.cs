using CityInfoAPI.Entities;

namespace CityInfoAPI
{
    public class CityDataStore
    {
        public List<City> Cities { get; set; }
        public static CityDataStore Current { get;}=new CityDataStore(); //there is no need to create object, with these we can able to access methods like instance
        public CityDataStore() 
        {
            Cities=new List<City>() {
                new City(){ Id=1,Name="Chennai",Description="Wonder ful place to live with cheap"},
                new City(){ Id=2,Name="Mumbai",Description="Wonder ful place for tourism but congested"},
                new City(){ Id=1,Name="Cochin",Description="Pleasant tourist place in india"}
            };
        }
    }
}
