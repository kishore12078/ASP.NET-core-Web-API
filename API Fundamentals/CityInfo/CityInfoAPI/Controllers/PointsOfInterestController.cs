using CityInfoAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities/pointsOfInterest/{cityId}")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointsOfInterest>> GetPointsOfInterests(int cityId)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city == null) 
                return NotFound();
            return Ok(city.PointsOfInterests);
        }

        [HttpGet("{pointOfInterestId}",Name ="GetPointOfInterest")]
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

        [HttpPost]
        public ActionResult<PointsOfInterest> PointOfInterestCreation(int cityId, PointOfInterestForCreationDTO pointOfInterest)
        {
            var city= CityDataStore.Current.Cities.FirstOrDefault(c=>c.Id == cityId);
            if(city == null)
                return NotFound();
            int maxPointOfInterestId = city.PointsOfInterests.Max(p => p.Id); //Finding the maximum id of pointofinterest for the particular city
            var newPointOfInterest = new PointsOfInterest() { Id = ++maxPointOfInterestId, //populating new pointofInterest object
                                                              Name = pointOfInterest.Name, 
                                                              Description = pointOfInterest.Description };
            city.PointsOfInterests.Add(newPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest",new { cityId=cityId,
                                                             pointOfInterestId = maxPointOfInterestId},
                                                       newPointOfInterest);
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult<PointsOfInterest> PointOfInterestUpdation([FromRoute] int cityId, 
                                                                      [FromRoute] int pointOfInterestId, 
                                                                      [FromBody] PointOfInterestForUpdationDTO pointOfInterest)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            var storePointOfInterest=city.PointsOfInterests.FirstOrDefault(p=> p.Id == pointOfInterestId);
            if(storePointOfInterest == null)
                return NotFound();
            storePointOfInterest.Name = pointOfInterest.Name;
            storePointOfInterest.Description = pointOfInterest.Description;
            return NotFound();
        }
    }
}
