using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Data;

public sealed class SeedService(ApplicationDbContext dbContext)
{
    public void ResetDatabase()
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }

    public async Task SeedData()
    {
        if (await dbContext.Roles.AnyAsync())
            return;

        foreach (var item in FakeDataFactory.Roles)
        {
            dbContext.Add(item);
        }
        await dbContext.SaveChangesAsync();

        foreach (var item in FakeDataFactory.Employees)
        {
            item.Role = await dbContext.Roles.FindAsync(item.Role.Id);
            dbContext.Add(item);
        }

        foreach (var item in FakeDataFactory.Preferences)
        {
            dbContext.Add(item);
        }
        await dbContext.SaveChangesAsync();

        foreach (var item in FakeDataFactory.Customers)
        {
            var preferenceIds = item.Preferences.Select(x => x.Id).ToArray();
            item.Preferences = await dbContext.Preferences
                                              .Where(x => preferenceIds.Contains(x.Id))
                                              .ToListAsync();
            dbContext.Add(item);
        }
        await dbContext.SaveChangesAsync();
    }
}
