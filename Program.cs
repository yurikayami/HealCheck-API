using HealCheckAPI.Data;
using HealCheckAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddHttpClient<IGeminiService, GeminiService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "HealCheck API",
        Version = "v1",
        Description = "Nutrition Analysis API using Gemini AI"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HealCheck API v1");
    });
}

// COMMENTED OUT: Disable HTTPS redirection for mobile device testing
// app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Serve static files from uploads directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "uploads")),
    RequestPath = "/uploads"
});

app.UseAuthorization();

app.MapControllers();

app.Run();
