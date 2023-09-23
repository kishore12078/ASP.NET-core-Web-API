using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Security.Cryptography;

namespace CityInfoAPI.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FileController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider= fileExtensionContentTypeProvider;
        }
        [HttpGet("{fileId}")]
        public ActionResult DownloadFile(string fileId)
        {
            var pathToFile = "BFSI.pdf";
            //check whether the file exists
            if(!System.IO.File.Exists(pathToFile))
                return NotFound();
            //make the content type to accept all the media type 
            if (_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes=System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes,contentType,Path.GetFileName(pathToFile));
        }
    }
}
