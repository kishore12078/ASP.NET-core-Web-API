using AutoMapper;
using CityInfoAPI.Entities;
using CityInfoAPI.Interfaces;
using CityInfoAPI.Models;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities/pointsOfInterest/{cityId}")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepo _cityRepo;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
                                          IMailService mailService,
                                          ICityInfoRepo cityRepo,
                                          IMapper mapper) //this is getting from the service container
        {
            _logger = logger??throw new ArgumentNullException(nameof(logger));
            _mailService=mailService ?? throw new ArgumentNullException(nameof(mailService));
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
        public async Task<ActionResult<PointsOfInterestDTO>> PointOfInterestCreation(int cityId, 
                                                                                     PointOfInterestForCreationDTO pointOfInterest)
        {
            if(!await _cityRepo.CheckCityExists(cityId))
                return NotFound();
            var result = _mapper.Map<PointOfInterest>(pointOfInterest);
            await _cityRepo.CreatePointOfInterest(cityId, result);
            result.CityId=cityId;
            var output = _mapper.Map<PointsOfInterestDTO>(result);
            return Created("PointOfInterest Added successfully", output);
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult<PointsOfInterestDTO> PointOfInterestUpdation([FromRoute] int cityId, 
                                                                      [FromRoute] int pointOfInterestId, 
                                                                      [FromBody] PointOfInterestForUpdationDTO pointOfInterest)
        {
            
            return NoContent();
        }

        //make sure while work with patch use another object like 'PointOfInterestForUpdationDTO' then only it couldn't replace the id property
        //[HttpPatch("{pointOfInterestId}")]
        //public ActionResult<PointsOfInterestDTO> PointOfInterestUpdationUsingPatch([FromRoute] int cityId,
        //                                                                        [FromRoute] int pointOfInterestId,
        //                                                                        JsonPatchDocument<PointOfInterestForUpdationDTO> jsonPatch)
        //{
        //    var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //    if (city == null)
        //        return NotFound();
        //    var storePointOfInterest = city.PointsOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId);
        //    if (storePointOfInterest == null)
        //        return NotFound();
        //    var oldPOI = new PointOfInterestForUpdationDTO
        //    {
        //        Name = storePointOfInterest.Name,
        //        Description = storePointOfInterest.Description
        //    };
        //    jsonPatch.ApplyTo(oldPOI, ModelState);
        //    if(!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    if(!TryValidateModel(oldPOI))
        //        return BadRequest(ModelState);
        //    storePointOfInterest.Name=oldPOI.Name;
        //    storePointOfInterest.Description=oldPOI.Description;
        //    return NoContent();
        //}

        //[HttpDelete("{pointOfInterestId}")]
        //public ActionResult DeletePointOfInterest(int cityId,int pointOfInterestId)
        //{
        //    var city = _dataStore.Cities.FirstOrDefault(c=>c.Id==cityId);
        //    if(city == null) return NotFound();
        //    var pointOfInterest= city.PointsOfInterests.FirstOrDefault(p=>p.Id==pointOfInterestId);
        //    if(pointOfInterest == null) return NotFound();
        //    city.PointsOfInterests.Remove(pointOfInterest);
        //    _mailService.Send("Point of Interest is Deleted", $"City id is {cityId} and Point of interest is {pointOfInterest}");
        //    return NoContent();
        //}

    }
}
