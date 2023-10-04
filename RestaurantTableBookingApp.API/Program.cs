
using LSC.RestaurantTableBookingApp.API;
using LSC.RestaurantTableBookingApp.API.Middleware;
using LSC.RestaurantTableBookingApp.API.PermissionValidation;
using LSC.RestaurantTableBookingApp.Data;
using LSC.RestaurantTableBookingApp.Data.BackgroundService;
using LSC.RestaurantTableBookingApp.Service;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using SendGrid;
using SendGrid.Extensions.DependencyInjection;
using Serilog;
using System.Net;
using System.Text.Json.Serialization;

namespace RestaurantTableBookingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog with the settings
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .CreateBootstrapLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                var configuration = builder.Configuration;

                builder.Services.AddApplicationInsightsTelemetry();

                builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                .WriteTo.ApplicationInsights(
                  services.GetRequiredService<TelemetryConfiguration>(),
                  TelemetryConverter.Events));

                Log.Information("Starting the application...");

                builder.Services.AddDbContext<RestaurantTableBookingDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DbContext"))
              //.EnableSensitiveDataLogging() //should not be used in production, only for development purpose
              );


                // Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApi(options =>

                        {
                            configuration.Bind("AzureAdB2C", options);
                            options.Events = new JwtBearerEvents();

                            /// <summary>
                            /// Below you can do extended token validation and check for additional claims, such as:
                            ///
                            /// - check if the caller's account is homed or guest via the 'acct' optional claim
                            /// - check if the caller belongs to right roles or groups via the 'roles' or 'groups' claim, respectively
                            ///
                            /// Bear in mind that you can do any of the above checks within the individual routes and/or controllers as well.
                            /// For more information, visit: https://docs.microsoft.com/azure/active-directory/develop/access-tokens#validate-the-user-has-permission-to-access-this-data
                            /// </summary>

                            //options.Events.OnTokenValidated = async context =>
                            //{
                            //    string[] allowedClientApps = { /* list of client ids to allow */ };

                            //    string clientAppId = context?.Principal?.Claims
                            //        .FirstOrDefault(x => x.Type == "azp" || x.Type == "appid")?.Value;

                            //    if (!allowedClientApps.Contains(clientAppId))
                            //    {
                            //        throw new System.Exception("This client is not authorized");
                            //    }
                            //};
                        }, options => { configuration.Bind("AzureAdB2C", options); });

                // The following flag can be used to get more descriptive errors in development environments
                IdentityModelEventSource.ShowPII = false;
               

                // Add services to the container.

              

                builder.Services.AddControllers()
                    .AddJsonOptions(options => {
                        // Ignore self reference loop
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    });

                // In production, modify this with the actual domains you want to allow
                builder.Services.AddCors(o => o.AddPolicy("default", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));

                //builder.Services.AddSendGrid(options =>
                //{
                //    options.ApiKey = configuration.GetValue<string>("SendGrid:SENDGRID_API_KEY");
                //});

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
                builder.Services.AddScoped<IRestaurantService, RestaurantService>();
                builder.Services.AddScoped<IReservationService, ReservationService>();
                builder.Services.AddScoped<IReservationRepository, ReservationRepository>();                
                builder.Services.AddScoped<IEmailNotification, EmailNotification>();
                builder.Services.AddScoped<IPermissionValidation, PermissionValidation>();
                builder.Services.AddHostedService<ReminderService>();

                var app = builder.Build();

                //if (builder.Environment.IsDevelopment())
                //{
                //    // Since IdentityModel version 5.2.1 (or since Microsoft.AspNetCore.Authentication.JwtBearer version 2.2.0),
                //    // Personal Identifiable Information is not written to the logs by default, to be compliant with GDPR.
                //    // For debugging/development purposes, one can enable additional detail in exceptions by setting IdentityModelEventSource.ShowPII to true.
                //    // Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                //    app.UseDeveloperExceptionPage();
                //}
                //else
                //{
                //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //    app.UseHsts();

                //}
                //Exception hanlding. Create a middleware and include that here
                // Enable Serilog exception logging
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        Log.Error(exception, "Unhandled exception occurred. {ExceptionDetails}", exception?.ToString());
                        Console.WriteLine(exception?.ToString());
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
                    });
                });

                app.UseMiddleware<RequestResponseLoggingMiddleware>();

                app.UseMiddleware<DelayMiddleware>();

                app.UseCors("default");
                // Configure the HTTP request pipeline.
                //if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}