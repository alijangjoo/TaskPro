namespace Suspect.TaskPro.Endpoints.Api;

using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using OysterFx.Endpoints.Api.Extensions.DI;
using OysterFx.Endpoints.Api.ModelBindings;
using OysterFx.Infra.Auth.JwtServices;
using Supect.TaskPro.Infra.InternalServices.UserApiCaller.Models;
using Supect.TaskPro.Infra.PanelPayamak.KaveNegarProvider;
using Suspect.TaskPro.AppCore.AppServices.AAA.Services;
using Suspect.TaskPro.AppCore.DomainServices.OtpCodeServices;
using Suspect.TaskPro.AppCore.Shared.AAA.Services;
using Suspect.TaskPro.AppCore.Shared.OtpCodeContracts.OtpCodes.DomainServices;
using Suspect.TaskPro.Endpoints.Api.Extensions;
using Suspect.TaskPro.Endpoints.Api.Extensions.Db;
using Suspect.TaskPro.Endpoints.Api.Extensions.Swaggers.Extentions;
using Suspect.TaskPro.Endpoints.Api.Middlewares;

public static class Hosting
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        IConfiguration configuration = builder.Configuration;

        builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        builder.Services.AddOysterFxApiCore("OysterFx", "Suspect.TaskPro", "Supect.TaskPro");
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddNonValidatingValidator();
        builder.Services.AddDatabase(configuration);
        //builder.Services.AddSingleton<IUserInfoService, FakeUserInfoService>();
        builder.Services.AddHealthCheckServices(configuration);
        //builder.Services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("Tasks", new OpenApiInfo
        //    {
        //        Title = "TaskPro – Tasks API",
        //        Version = "v1"
        //    });
        //    c.SwaggerDoc("Wrappers", new OpenApiInfo
        //    {
        //        Title = "TaskPro – Wrappers API",
        //        Version = "v1"
        //    });
        //    c.SwaggerDoc("Auth", new OpenApiInfo
        //    {
        //        Title = "TaskPro – Auth API",
        //        Version = "v1"
        //    });
        //});

        builder.Services.AddSwaggerDocumentation("TaskPro API", "v1");

        builder.Services.AddKavehNegarPayamk(options =>
        {
            options.ServicePath = builder.Configuration["PanelPayamak:Providers:KavehNegar:ServicePath"];
        });

        builder.Services.AddUserServices(builder.Configuration);
        builder.Services.AddTransient<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
        builder.Services.AddTransient<IOtpGeneratorDomainService, OtpGeneratorDomainService>();
        builder.Services.AddJwtAuthentication(configuration);
        builder.Services.AddJwtUserInfoService();
        builder.Services.AddRateLimitServices(configuration);
        return builder.Build();

    }
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseMiddleware<ApiKeyMiddleware>();
        app.UseOysterFxApiExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/Tasks/swagger.json", "Tasks API");
            //    c.SwaggerEndpoint("/swagger/Wrappers/swagger.json", "Wrappers API");
            //    c.SwaggerEndpoint("/swagger/Auth/swagger.json", "Auth API");
            //});

            app.UseSwaggerMiddleware("TaskPro API v1");
        }
        app.UseMiddleware<BusinessRuleExceptionMiddleware>();
        app.UseStatusCodePages();

        app.UseCors(delegate (CorsPolicyBuilder builder)
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        });

        app.UseHttpsRedirection();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,  // Include all checks
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("database"),  // Only include external dependencies
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false  // No checks - just return 200 if service is reachable
        });

        if (bool.Parse(app.Configuration["Jwt:Enabled"]!))
        {
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseTokenProcessing();
        }

        var controllerBuilder = app.MapControllers();
        return app;
    }
}
