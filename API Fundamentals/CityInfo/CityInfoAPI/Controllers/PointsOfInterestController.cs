using AutoMapper;
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
        private readonly ICityInfoRepo _cityRepo;
        private readonly IMapper _mapper;
        private readonly CityDataStore? _dataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
                                          IMailService mailService,
                                          ICityInfoRepo cityRepo,
                                          IMapper mapper) //this is getting from the service container
        {
            _logger = logger??throw new ArgumentNullException(nameof(logger));
            _mailService=mailService ?? throw new ArgumentNullException(nameof(mailService));
            //_dataStore=dataStore?? throw new ArgumentNullException(nameof(mailService));
            _cityRepo = cityRepo ?? throw new ArgumentNullException(nameof(cityRepo));
            _mapper=mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointsOfInterestDTO>>> GetPointsOfInterests(int cityId)
        {
            if (!await _cityRepo.CheckCityExists(cityId))
            {
                _logger.LogCritical("Not Found");
                return NotFound();
            }
            var pointOfInterests = await _cityRepo.GetPointOfInterestsAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointsOfInterestDTO>>(pointOfInterests));
        }

        [HttpGet("{pointOfInterestId}",Name ="GetPointOfInterest")]
        public async Task<ActionResult<PointsOfInterestDTO>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityRepo.CheckCityExists(cityId))
            {
                _logger.LogCritical("Not Found");
                return NotFound();
            }
            var pointOfInterest = await _cityRepo.GetPointOfInterestAsync(cityId,pointOfInterestId);
            if (pointOfInterest == null) return NotFound();
            return Ok(_mapper.Map<PointsOfInterestDTO>(pointOfInterest));
        }

        [HttpPost]
        public ActionResult<PointsOfInterestDTO> PointOfInterestCreation(int cityId, PointOfInterestForCreationDTO pointOfInterest)
        {
            var city= _dataStore.Cities.FirstOrDefault(c=>c.Id == cityId);
            if(city == null)
                return NotFound();
            int maxPointOfInterestId = city.PointsOfInterests.Max(p => p.Id); //Finding the maximum id of pointofinterest for the particular city
            var newPointOfInterest = new PointsOfInterestDTO() { Id = ++maxPointOfInterestId, //populating new pointofInterest object
                                                              Name = pointOfInterest.Name, 
                                                              Description = pointOfInterest.Description };
            city.PointsOfInterests.Add(newPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest",new { cityId=cityId,
                                                             pointOfInterestId = maxPointOfInterestId},
                                                       newPointOfInterest);
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult<PointsOfInterestDTO> PointOfInterestUpdation([FromRoute] int cityId, 
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
        public ActionResult<PointsOfInterestDTO> PointOfInterestUpdationUsingPatch([FromRoute] int cityId,
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
