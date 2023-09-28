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
## POST attribute
* the json data is get from request body if it is not in the json format JSON input and output formatters automatically converted it into JSON.
* No need to mention [FromBody] in the parameter passing
* we must wrote validation for the model's property to avoid unneccesary errors and intimate the consumer that what goes wrong.
```C#
public class PointOfInterestForCreationDTO
    {
        [Required(ErrorMessage ="You Should Provide Name of the City")]
        [MaxLength(20,ErrorMessage ="Name should be Maximum of 20 characters")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200,ErrorMessage ="Description should be maximum of 200 Characters")]
        public string Description { get; set; } = string.Empty;
    }
```
* PUT is almost same to post but it updates the previous value into new but what the challenge here is, it should change all the property values eventhough if the user couldn't gave it.
* For example user wants to change the name property only so he gave new name and leave description as empty, but in the output name is get changed but description field goes null.
* we can achieve this challenge by PATCH which is partially updation attribute.

## PATCH attribute
* 1st we need install 'Microsoft.aspnetcore.jsonPatch' package to work with patch.
* In the input taking parameter use Patchdocument with required object.
```C#
[HttpPatch("{pointOfInterestId}")]
        public ActionResult<PointsOfInterest> PointOfInterestUpdationUsingPatch([FromRoute] int cityId,
                                                                                [FromRoute] int pointOfInterestId,
                                                                                JsonPatchDocument<PointOfInterestForUpdationDTO> jsonPatch)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
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
```
* Once getting the required object from store and make it as a object that we specified in the Patch document.
```C#
var oldPOI = new PointOfInterestForUpdationDTO
            {
                Name = storePointOfInterest.Name,
                Description = storePointOfInterest.Description
            };
```
* then by 'ApplyTo()' method pass the oldPOI to the patch document, 1st it will check if the patch document is valid model and replace the value accordingly.
* Again we need to check the output object that it is valid or not
```C#
if(!TryValidateModel(oldPOI))
                return BadRequest(ModelState);
``` 
* Then populate the storeObject and it will get partially updated.
## Dependency Injection
* Traditional project workflow follows tightly coupling that is create the object of particular class or service in the controller.
```C#
public class ProductService
{
    private readonly ILogger _logger;
    
    public ProductService()
    {
        _logger = new Logger();
    }
    
    public void DoSomething()
    {
        _logger.Log("Doing something");
    }
}
```
* IoC (Inversion Of Control) and dependency injection decouples the coupling and just inject the services container into the constructor of controller.
```C#
public class ProductService
{
    private readonly ILogger _logger;
    
    public ProductService(ILogger logger)
    {
        _logger = logger;
    }
    
    public void DoSomething()
    {
        _logger.Log("Doing something");
    }
}
```
* why we check null during assigning of readonly variable?
    * Because if in case we change the service container from the program class but using the same interface in the controller results returning null
* In appSettings.json <mark>Default: "Information"</mark> represents information are printed on console and <mark>Default: "Warning"</mark> represents nothing will be printed on console.
* what ever log from controller we can make that to be print in console by adding `"CityInfoAPI.Controllers": "Information"` in the appSettings.json.
* Serilog is one the custom logger used in ASP.NET core to keep track of activities.
```C#
#region Serilog Configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/CityInfo.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog(); //remove the default logger and intimate the custom logger

            #endregion
```
* From Package manager console window itself we will able to dowload package by `install-package <package name>`.
* we can utilize the services based on the environments in visual studio
```C#
#if DEBUG //if visual studio in the debug environments
builder.Services.AddTransient<IMailService, LocalMailService>();
#else //if visual studio is in the release or producton environments
builder.Services.AddTransient<IMailService, LocalMailService>();
#endif
```
* IConfiguration is already registered in service container so no need to register it again.
```C#
public CloudMailService(IConfiguration configuration)
{
    _to = configuration["mailSettings:toMail"];
    _from = configuration["mailSettings:fromMail"];
}
```
* These configuration object is different for environments, we cannot use the developments configuration file in production.
* For every environments we need to create separate appsettings.json file for example of Production environments that file should be `appsettings.Production.json`.
* After this it will automatically added within the appsettings.json file, and the default file is `appsettings.Development.json`.

## Entity Framework
### Object-Relational Mapping
&emsp;&emsp; With the help of object in c# we can determine the relations between tables in database and we can process the data from the database with some queries by object-oriented format.
* We should create separate class for entities which mimic the models class.
* Don't need to include the calculated fields in entity class because we don't want that to be inserted in database.
* Instead of giving `string.Empty` create `parameterized constructor` and initialize that there so that we should intimate the user, the name of the city should be present whenever they create the instance.
* `Identity` implies increment while adding new record and `Computed` implies changing the previous record that is used in update.
* Entity class property have annotations like [Key] [ForeignKey] while models class have validation annotations.
