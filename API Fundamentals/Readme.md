# API Fundamentals
 * ASP.NET core is framework for developing things and .NET or .NET core is the platform to run.
 * Only One dependency is referenced defaultly named SwashBuckle.AspNetCore which is nothing but Swagger (Open API).
* LaunchSettings.json is creating environments for local machine development.
* .NET CLI (Command Line Interface) is a tool used to create, build, run, publish the .NET applications and able to do whatever we could do in the Visual Studio. 
* Services is a component that intended to a common consumption wherever from in the code.
* API program class doesn't have a main method and namespace it have something weired named Top-Level Statements, it will automatically generate main method().
* var app = builder.Build(); is build the application and create webApplication object and WebApplication builder inherit IApplicationBuilder.
* Those components are process the HTTP Requests are Middleware.
* If we have two middlewares -> authentication,Routing -> if authentication is successfull means it will pass into the next pipeline or else it will give back the response.
* so the order that we construct the pipeline is important
* Middleware pipeline is only added in the development environment
## EndPoints Routing
* Controller parse the URI from frontend and try to match with the equivalent endPoints or Action method
* To achieve this we have inject two middlewares in pipeline
    * app.UseRouting()->make decision of which endpoints to be select
    * app.UseEndPoints(endPoints=>{endPoints.MapController})->execute the selected endpoint.
* Instead of do like this just app.UseRouting(), app.UseAuthorization(), app.MapControllers()=> it having IEndPoint interface. -> these way is called Convention based.
* but ASP.NET core 6 introduce Attribute based endpoints routing, this will achieve by GET,POST,PUT,DELETE attributes.
## Importance of Status Codes
* Level 200(Success) -> 200Ok, 200Created, 204No Content 
* Level 400(Client Side Mistake) -> 400BadRequest => may be JSON isn't the correct form, 401 UnAuthorised, 403 Forbidden => authenticated passed but the particular user is not eligible for access, 404 Not found, 409 Conflict.
* Level 500 (Server side error) -> 500 Internal Server Error => no way just user wants to try again.
## Formatters and Negotiation
* API defaulty return JSON as output and accept json as input
* If some client asks xml as output format means api will check for the xml format if not found means it will return the default format that is json.
* So if we add 
```c#
{
    builder.Services.AddControllers(options =>
            options.ReturnHttpNotAcceptable = true
                );
    // if api doesn't have json it will return 406 NotAcception Action result

    builder.Services.AddControllers(options =>
            options.ReturnHttpNotAcceptable = true
                ).AddXmlDataContractSerializerFormatters();
    // this code will serialize the json or any other format to xml if the client asks xml as output format
}
```
## Working on File with API
* FileContentType,FileStreamType,VirtualFileResult,PhysicalFileResult all we can utilize while working on files.
* first make sure the file which gonna download are loaded in same directory and go to its property and make "don not copy" to "copy all".
* with the fileId get the control of file and check if the file is exist or not
```c#
var pathToFile = "BFSI.pdf";
//check whether the file exists
if(!System.IO.File.Exists(pathToFile))
    return NotFound();
```
* defaultly contentt-type is "text/plain" this will not accept for all the media type so we need to tell it as explicitly to accept all kind of media type by inject 
```c#
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
``` 
in the program class.

```c#
if (_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes=System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes,contentType,Path.GetFileName(pathToFile));
```
with these piece of code we can download the file which of any type from swagger.



