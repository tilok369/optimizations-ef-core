using Bogus;
using EFCore.Optimizations.Application.Domain;

namespace EFCore.Optimizations.Application;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext)
    {
        //if (!dbContext.Profiles.Any())
        {
            var profileFaker = new Faker<Profile>()
                .RuleFor(r=>r.Name, f=>f.Name.FullName())
                .RuleFor(r=>r.Email, f => f.Internet.Email())
                .RuleFor(r=>r.Phone, f => f.Phone.PhoneNumber("(###) ###-####"))
                .RuleFor(r=>r.Address, f => f.Address.StreetAddress());
            
            var profiles = profileFaker.Generate(10000);
            
            var userFake = new Faker<User>()
                .RuleFor(r => r.UserName, f => f.Person.UserName)
                .RuleFor(r => r.Password, f => f.Random.AlphaNumeric(10))
                .RuleFor(r=>r.ProfileId, f => f.PickRandom(profiles).Id);
            
            var users = userFake.Generate(100);

            foreach (var user in users)
            {
                var profile = profiles.First(x => x.Id == user.ProfileId);
                user.Profile = profile;
            }

            dbContext.AddRange(profiles);
            dbContext.AddRange(users);
            await dbContext.SaveChangesAsync();
        }
    }
}