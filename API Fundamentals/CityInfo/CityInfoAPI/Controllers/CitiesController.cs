using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        public JsonResult GetCitites()
        {
            return new JsonResult(
                new List< object >{
                    new { Id = 1,Name="Chennai" },
                    new {Id = 2,Name="Mumbai" }
            });
        }
    }
}
