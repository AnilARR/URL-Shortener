using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Dal;
using UrlShortenerApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("UrlShortenerDb");

AppContext.SetSwitch("System.Globalization.Invariant", false);
builder.Services.AddDbContext<ShortDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Add CORS policy to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<Repository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowAllOrigins");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
