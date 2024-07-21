using DaMid.Contexts;
using DaMid.Interfaces.Options;
using DaMid.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

foreach (var file in Directory.EnumerateFiles(Environment.CurrentDirectory, "appsettings.*.json")) {
    builder.Configuration.AddJsonFile(file);
}

builder.Services.Configure<IJwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<IDatabaseOptions>(builder.Configuration.GetSection("Database"));

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<ApplicationContext>(options => {
    options.UseMySQL(builder.Configuration.GetValue<string>("Database:ConnectionString"));
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();
app.UseSwaggerUI();
app.UseSwagger();

app.Run();
