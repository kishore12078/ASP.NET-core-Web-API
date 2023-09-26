﻿using CityInfoAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<City>> GetCitites()
        {
            //return new JsonResult(
            //    new List< object >{
            //        new { Id = 1,Name="Chennai" },
            //        new {Id = 2,Name="Mumbai" }
            //});
            CityDataStore cityDataStore = new CityDataStore();
            if(cityDataStore.Cities != null) 
                return Ok(cityDataStore.Cities);
            return NoContent();
            //return new JsonResult(cityDataStore.Cities);
            //return new JsonResult(CityDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<City> GetCityById(int id)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
                return NotFound();
            return Ok(city);   
            //return new JsonResult(CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == id));
        }
    }
}
