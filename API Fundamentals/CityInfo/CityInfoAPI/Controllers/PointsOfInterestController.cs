using CityInfoAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities/pointsOfInterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet("{cityId}")]
        public ActionResult<IEnumerable<PointsOfInterest>> GetPointsOfInterests(int cityId)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city == null) 
                return NotFound();
            return Ok(city.PointsOfInterests);
        }

        [HttpGet("{cityId}/{pointOfInterestId}")]
        public ActionResult<PointsOfInterest> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(c=> c.Id == cityId);
            if(city == null)
                return NotFound();
            var pointOfInterest= city.PointsOfInterests.FirstOrDefault(p=> p.Id == pointOfInterestId);
            if(pointOfInterest == null)
                return NotFound();
            return Ok(pointOfInterest);
        }
    }
}
