﻿using AutoMapper;
using CityInfoAPI.Entities;
using CityInfoAPI.Interfaces;
using CityInfoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfoAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        //private readonly CityDataStore? _dataStore;
        private readonly ICityInfoRepo _cityRepo;
        private readonly IMapper _mapper;
        const int maxPageSize = 10;

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

        /// <summary>
        /// Get a city based on the Id which was given by user
        /// </summary>
        /// <param name="id">Pass the Id </param>
        /// <param name="includePointOfInterest">Decide whether pointOfInterest to be include or not</param>
        /// <returns>CityDTO or CityDTOwithoutpointofinterest</returns>
        /// <response code="200">Returns the requested city</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

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
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CitiesWithoutPointOfInterestsDTO>>> CityFiltering([FromQuery] string? name,
                                                                                                     [FromQuery] string? queryName,
                                                                                                     [FromQuery] int pageSize=5,
                                                                                                     [FromQuery] int pageNumber=1)
        {
            if(pageSize>maxPageSize)
                pageSize = maxPageSize;
            var (cities,pagenationMetaData) = await _cityRepo.CityFiltering(name,queryName,pageSize,pageNumber);
            Response.Headers.Add("X-Pagenation", JsonSerializer.Serialize(pagenationMetaData));//it will add metadata into the headers
            if (cities.Count() <= 0)
                return NotFound();
            return Ok(_mapper.Map<IEnumerable<CitiesWithoutPointOfInterestsDTO>>(cities));
        }
    }
}
