using System.Diagnostics;
using EFCore.Optimizations.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Optimizations.Application;

public static class Comparator
{
    public static void ToListThenFilter(ApplicationDbContext dbContext)
    {
        Console.WriteLine("ToListThenFilter");
        Stopwatch watch = new Stopwatch();
        watch.Start();
        var profiles = dbContext.Profiles.ToList().Select(p=>p.Email.EndsWith(".gmail.com"));
        watch.Stop();
        Console.WriteLine($"ToListThenFilter: {watch.ElapsedMilliseconds}ms");
    }
    
    public static void FilterThenToList(ApplicationDbContext dbContext)
    {
        Console.WriteLine("FilterThenToList");
        Stopwatch watch = new Stopwatch();
        watch.Start();
        var profiles = dbContext.Profiles.Where(p=>p.Email.EndsWith(".gmail.com")).ToList();
        watch.Stop();
        Console.WriteLine($"ToListThenFilter: {watch.ElapsedMilliseconds}ms");
    }

    public static void WithEagerLoading(ApplicationDbContext dbContext)
    {
        Console.WriteLine("WithEagerLoading");
        var profiles = dbContext.Profiles.Take(10).Include(p=>p.User).ToList();
        foreach (var profile in profiles)
        {
            var userName = profile.User?.UserName??"";
            Console.WriteLine($"User Name: {userName}");
        }
    }
    
    public static void WithLazyLoading(ApplicationDbContext dbContext)
    {
        Console.WriteLine("WithLazyLoading");
        var profiles = dbContext.Profiles.Take(10).ToList();
        foreach (var profile in profiles)
        {
            var user = profile.User;
            var userName = user?.UserName??"";
            Console.WriteLine($"User Name: {userName}");
        }
    }
}