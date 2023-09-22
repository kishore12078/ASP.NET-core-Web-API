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

