
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Extensions;
using System.Text;
using TTVMAMobileWebMiddleware.Application.Interfaces;
using TTVMAMobileWebMiddleware.Application.Interfaces.Mobile;
using TTVMAMobileWebMiddleware.Application.Services;
using TTVMAMobileWebMiddleware.Domain.Entities;
using TTVMAMobileWebMiddleware.Infrastructure.ExternalAPIServices;
using TTVMAMobileWebMiddleware.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

Shared.ShrStartup.RegisterServices(builder.Services, builder.Configuration);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<ExternalApiSettings>(builder.Configuration.GetSection("ExternalApi"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("http://localhost:7188",
                                "http://localhost:7188",
                                "http://localhost:7288",
                                "http://localhost:5002",
                                "http://localhost:4000",
                                "http://localhost:3000")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Pagination-MetaData");
        });
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<MOBDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TTVMAMobileConnection")));

builder.Services.AddDbContext<DLSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TTVMAConnection")));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "TTVMA.Mobile.MiddleWear API",
        Version = "v1"
    });

    var baseDir = builder.Configuration["Swagger:baseDir"] ?? AppDomain.CurrentDomain.BaseDirectory;
    var docFiles = new[] { "MobieAPIDoc.xml" };

    foreach (var doc in docFiles)
    {
        var path = Path.Combine(baseDir, doc);
        if (File.Exists(path))
            options.IncludeXmlComments(path);
    }

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and the JWT token"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("api-version")
    );
}).AddMvc()
  .AddApiExplorer(options =>
  {
      options.GroupNameFormat = "'v'VVV";
      options.SubstituteApiVersionInUrl = true;
      options.DefaultApiVersion = new ApiVersion(1, 0);
      options.AssumeDefaultVersionWhenUnspecified = true;
  });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddScoped<IVillageService, VillageService>();
builder.Services.AddScoped<ICitizenService, CitizenService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<INotificationService,  NotificationService>();

// Background worker services
builder.Services.AddHostedService<TTVMAMobileWebMiddleware.Application.Workers.ReceiptSyncWorker>();

// External API HTTP Client
builder.Services.AddHttpClient<IExternalApiService, ExternalApiService>();
// JWT auth
var jwt = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;


builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();

