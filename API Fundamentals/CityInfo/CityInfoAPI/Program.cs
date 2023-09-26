using CityInfoAPI.Controllers;
using Microsoft.AspNetCore.StaticFiles;
using Serilog;

namespace CityInfoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //#region Adding custom Logging methods
            //builder.Logging.ClearProviders(); //remove the predefined functions from the logging
            //builder.Logging.AddConsole(); //add custom logging method
            //#endregion

            #region Serilog Configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/CityInfo.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog(); //remove the default logger and intimate the custom logger

            #endregion

            // Add services to the container.

            builder.Services.AddControllers(options =>
            options.ReturnHttpNotAcceptable = true
                ).AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters() ;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            //app.MapControllers();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            app.Run();
        }
    }
}