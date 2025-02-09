// See https://aka.ms/new-console-template for more information

using EFCore.Optimizations.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));


using var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//await DataSeeder.SeedAsync(dbContext);

//Comparator.ToListThenFilter(dbContext);
//Comparator.FilterThenToList(dbContext);

Comparator.WithEagerLoading(dbContext);
Comparator.WithLazyLoading(dbContext);

Console.WriteLine("Operations completed");