using API.Data;
using API.Extensions;
using API.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Add services to the container.

services.AddControllers();

services.AddApplicationServices();

services.AddAutoMapper(typeof(MappingProfiles));

services.AddDbContext<GrossToNetContext>(
    dbContextOptions => dbContextOptions
        .UseSqlServer(config["ConnectionStrings:MSSQL"])
);

var app = builder.Build();

await Migration.Migrate(app);

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
