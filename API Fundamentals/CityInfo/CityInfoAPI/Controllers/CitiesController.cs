using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public JsonResult GetCitites()
        {
            //return new JsonResult(
            //    new List< object >{
            //        new { Id = 1,Name="Chennai" },
            //        new {Id = 2,Name="Mumbai" }
            //});
            CityDataStore cityDataStore = new CityDataStore();
            return new JsonResult(cityDataStore.Cities);
            //return new JsonResult(CityDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public JsonResult GetCityById(int id)
        {
            return new JsonResult(CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == id));
        }
    }
}
