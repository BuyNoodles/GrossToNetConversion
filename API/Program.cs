using API.Extensions;
using API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));

services.AddDbContext<GrossToNetContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(config["MySql:ConnectionString"], serverVersion)
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
