﻿using CityInfoAPI.Entities;
using CityInfoAPI.Interfaces;
using CityInfoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly CityDataStore? _dataStore;
        private readonly ICityInfoRepo _cityRepo;

        //public CitiesController(CityDataStore dataStore)
        //{
        //    _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        //}
        public CitiesController(ICityInfoRepo cityRepo)
        {
            _cityRepo=cityRepo ?? throw new ArgumentNullException(nameof(cityRepo));
        }
        [HttpGet]
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


            var cities=await _cityRepo.GetCitiesAsync();
            var results = new List<CitiesWithoutPointOfInterestsDTO>();
            foreach (var city in cities)
            {
                results.Add(new CitiesWithoutPointOfInterestsDTO 
                { Id = city.Id, 
                  Description = city.Description, 
                  Name = city.Name 
                });
            }
            return Ok(results);
        }

        //[HttpGet("{id}")]
        //public ActionResult<CityDTO> GetCityById(int id)
        //{
        //    var city = _dataStore.Cities.FirstOrDefault(c => c.Id == id);
        //    if (city == null)
        //        return NotFound();
        //    return Ok(city);   
        //    //return new JsonResult(CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == id));
        //}
    }
}
