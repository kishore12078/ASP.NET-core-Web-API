using AutoMapper;
using CityInfoAPI.Entities;
using CityInfoAPI.Interfaces;
using CityInfoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        //private readonly CityDataStore? _dataStore;
        private readonly ICityInfoRepo _cityRepo;
        private readonly IMapper _mapper;

        //public CitiesController(CityDataStore dataStore)
        //{
        //    _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        //}
        public CitiesController(ICityInfoRepo cityRepo,IMapper mapper)
        {
            _cityRepo=cityRepo ?? throw new ArgumentNullException(nameof(cityRepo));
            _mapper=mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet("allCities")]
        public async Task<ActionResult<IEnumerable<CitiesWithoutPointOfInterestsDTO>>> GetCitites()
        {
            //return new JsonResult(
            //    new List< object >{
            //        new { Id = 1,Name="Chennai" },
            //        new {Id = 2,Name="Mumbai" }
            //});
            //CityDataStore cityDataStore = new CityDataStore();
            //if(cityDataStore.Cities != null) 
            //    return Ok(cityDataStore.Cities);
            //return NoContent();
            //return new JsonResult(cityDataStore.Cities);
            //return new JsonResult(CityDataStore.Current.Cities);


            var cities = await _cityRepo.GetCitiesAsync();
            var results = _mapper.Map<IEnumerable<CitiesWithoutPointOfInterestsDTO>>(cities);
            //var results = new List<CitiesWithoutPointOfInterestsDTO>();
            //foreach (var city in cities)
            //{
            //    results.Add(new CitiesWithoutPointOfInterestsDTO
            //    {
            //        Id = city.Id,
            //        Description = city.Description,
            //        Name = city.Name
            //    });
            //}
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityById(int id,bool includePointOfInterest=false)
        {
            var city = await _cityRepo.GetCityByIdAsync(id,includePointOfInterest);
            if(city == null) return NotFound();
            if (includePointOfInterest)
                return Ok(_mapper.Map<CityDTO>(city));
            return Ok(_mapper.Map<CitiesWithoutPointOfInterestsDTO>(city));
            //return new JsonResult(CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == id));
        }


        //Filtering
        [HttpGet("filter/{name}")]
        public async Task<ActionResult<IEnumerable<CitiesWithoutPointOfInterestsDTO>>> CityFiltering([FromRoute] string? name)
        {
            var cities = await _cityRepo.CityFiltering(name);
            if (cities.Count() <= 0)
                return NotFound();
            return Ok(_mapper.Map<IEnumerable<CitiesWithoutPointOfInterestsDTO>>(cities));
        }
    }
}
