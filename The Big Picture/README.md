# ASP.NET-core-Web-API
I am learning ASP.NET core web API deeply with hands On and doing projects  

THE BIG PICTURE:
What is ASP.NET core and why it is needed?
-> It is an open source, both microsoft and community works on it
-> High performance
-> Cross platform , it can be host in lot of platforms such as windows and linux
-> Modular

Difference Between Website and WebApplication:
website is a static page that shows content to the user not changes anything it have by Html and css
on the other hand WebApplication is to show content to the user that is getting from the backend and dynamically changes

developers design and do lot of structural and functional things in the backend with ASP.NET core webapplication framework and just expose the only necessary things to the user.
->One backend application can reuse to lot of other frontend's or backend application

Tool:
visual studio code -> microsoft -> different versions for windows and mac and some of the feature in windows are not present in mac
Rider -> jetbrains -> cross platforms one applications can able to use in different os

Once the application is started execution starts at pragram.cs first builder object is created and by use this get the app and then app.Run() is execute once the it get run, the command line application has turned into ASP.NET core application.
-> after running, it will render in the browser and the browser communicate to the application through a server
-> visual studio having built in server named 'kestrel' our application is hosted on that server and browser took out application from the server
-> in case of production webBrowser needs to communicate to lot of webApplications so to achieve this IIS(windows) and NGINX(Linux) came into the picture
-> browser communicate to IIS and IIS to kestrel and response flow is kestrel to IIS to Browser.

DEPENDENCY INJECTION:
-> Dependency injection container is the central part where types can be registered and it manages the life time of objects

MIDDLEWARE AND REQUEST PIPELINE:
-> the kestrel request goes into middleware pipeline it have authorization, routing and staticfiles(html,css,javascript) extension methods. if the application doesn't have middleware pipelines then it won't process the requesst and couldn't produce response.

RAZOR:
-> it a html page and a option to embed with c#

OBJECT LIFTIMES:
In dependency injection, AddSingleton -> create single instance for entire application
AddTransient -> every time when type is called the same instance is get override instead of create new instance
AddScoped -> for every registered type new instance is created for the entire application

SERVER-RENDERED FRONTEND APPLICATION
-> when browser makes request to server, server processing the request and give back the pure html page as response to the browser
-> asp-page is called tag-helpers, this attribute took the name and append it in url

[BindProperty] -> annotation helps populate data automatically to the objects

MVC:
-> request will hit first is controller class and has a method named Action.

CLIENT RENDERED FRONTEND APPLICATION:
-> Blazor WebAssembly and Blazor Server
-> client rendered applications are fall into the category of Single Page Application(SPA)
-> components in Blazor is written by Razor and C# and reused over and over again in the application
-> browser make a request to server and it has a UserInterface written by Razor and C#
-> Blazor code will execute in the browser 
