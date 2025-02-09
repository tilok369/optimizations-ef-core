// See https://aka.ms/new-console-template for more information

using EFCore.Optimizations.Application;
using EFCore.Optimizations.Application.Domain;
using EFCore.Optimizations.Application.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);


builder.Services.AddSingleton<AuditInterceptor>();
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
        .AddInterceptors(serviceProvider.GetRequiredService<AuditInterceptor>()));


using var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//await DataSeeder.SeedAsync(dbContext);

//Comparator.ToListThenFilter(dbContext);
//Comparator.FilterThenToList(dbContext);

//Comparator.WithEagerLoading(dbContext);
//Comparator.WithLazyLoading(dbContext);

dbContext.Profiles.Add(new Profile
{
    Name = "Test 1",
    Email = "test1@gmail.com",
    Phone = "+8801123456",
    Address = "Test Address 1"
});
await dbContext.SaveChangesAsync();

Console.WriteLine("Operations completed");