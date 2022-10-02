
using Application;
using Application.Common.Interfaces;
using Application.Quizs.Commands;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;

using WebApi.Filter;
using WebApi.OperationProcessors;
using WebApi.Service;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.ConfigureServices(builder.Configuration);

    var app = builder.Build();
    app.MapControllers();

    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        //Only for Code first applications
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHealthChecks("/health");
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseOpenApi(options =>
    {
        options.DocumentName = "specification";
        options.Path = "/swagger/specification.json";
    });
    app.UseSwaggerUi3(setting =>
    {
        setting.Path = "/swagger";
        setting.DocumentPath = "/swagger/specification.json";
    });
    app.UseReDoc(options =>
    {
        options.Path = "/redoc";
        options.DocumentPath = "/swagger/specification.json";
    });

    app.Run();
}
catch
{
}
finally
{

}



public static class Configuration
{
    public static void ConfigureServices(this WebApplicationBuilder builder,
            IConfiguration configuration)
    {
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(configuration);


        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

        builder.Services.Configure<AuthConfiguration>(configuration.GetSection("AuthConfiguration"));

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationRepository>();
        var authConfiguration = configuration.GetSection("AuthConfiguration").Get<AuthConfiguration>();

        builder.Services.AddJwtAuthentication(authConfiguration);

        builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());
         
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateQuizCommandValidator>();



        builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });


        builder.Services.AddOpenApiDocument(configure =>
        {
            configure.DocumentName = "specification";
            configure.Title = "Quiz API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            configure.OperationProcessors.Insert(0, new IncludeControllersInSwagger());
        });
    }


}
