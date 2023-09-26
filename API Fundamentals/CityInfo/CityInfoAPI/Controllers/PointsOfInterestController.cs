using CityInfoAPI.Entities;
using CityInfoAPI.Interfaces;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities/pointsOfInterest/{cityId}")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CityDataStore _dataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
                                          IMailService mailService,
                                          CityDataStore dataStore) //this is getting from the service container
        {
            _logger = logger??throw new ArgumentNullException(nameof(logger));
            _mailService=mailService ?? throw new ArgumentNullException(nameof(mailService));
            _dataStore=dataStore?? throw new ArgumentNullException(nameof(mailService));
        }
        [HttpGet]
        public ActionResult<IEnumerable<PointsOfInterest>> GetPointsOfInterests(int cityId)
        {
            try
            {
                //throw new Exception("Exception");
                var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    _logger.LogInformation($"You requested cityId {cityId} is not found");
                    return NotFound();
                }
                return Ok(city.PointsOfInterests);
            }
            catch (Exception)
            {
                _logger.LogCritical("Exception occued");
                return StatusCode(500, "Exception occured while processing your request");
            }
        }

        [HttpGet("{pointOfInterestId}",Name ="GetPointOfInterest")]
        public ActionResult<PointsOfInterest> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _dataStore.Cities.FirstOrDefault(c=> c.Id == cityId);
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
            var city= _dataStore.Cities.FirstOrDefault(c=>c.Id == cityId);
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
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            var storePointOfInterest=city.PointsOfInterests.FirstOrDefault(p=> p.Id == pointOfInterestId);
            if(storePointOfInterest == null)
                return NotFound();
            storePointOfInterest.Name = pointOfInterest.Name;
            storePointOfInterest.Description = pointOfInterest.Description;
            return NoContent();
        }

        //make sure while work with patch use another object like 'PointOfInterestForUpdationDTO' then only it couldn't replace the id property
        [HttpPatch("{pointOfInterestId}")]
        public ActionResult<PointsOfInterest> PointOfInterestUpdationUsingPatch([FromRoute] int cityId,
                                                                                [FromRoute] int pointOfInterestId,
                                                                                JsonPatchDocument<PointOfInterestForUpdationDTO> jsonPatch)
        {
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            var storePointOfInterest = city.PointsOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (storePointOfInterest == null)
                return NotFound();
            var oldPOI = new PointOfInterestForUpdationDTO
            {
                Name = storePointOfInterest.Name,
                Description = storePointOfInterest.Description
            };
            jsonPatch.ApplyTo(oldPOI, ModelState);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!TryValidateModel(oldPOI))
                return BadRequest(ModelState);
            storePointOfInterest.Name=oldPOI.Name;
            storePointOfInterest.Description=oldPOI.Description;
            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId,int pointOfInterestId)
        {
            var city = _dataStore.Cities.FirstOrDefault(c=>c.Id==cityId);
            if(city == null) return NotFound();
            var pointOfInterest= city.PointsOfInterests.FirstOrDefault(p=>p.Id==pointOfInterestId);
            if(pointOfInterest == null) return NotFound();
            city.PointsOfInterests.Remove(pointOfInterest);
            _mailService.Send("Point of Interest is Deleted", $"City id is {cityId} and Point of interest is {pointOfInterest}");
            return NoContent();
        }
    }
}
