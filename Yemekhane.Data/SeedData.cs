using Yemekhane.Entities;

namespace Yemekhane.Data;

public class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (!context.Meals.Any())
        {
            context.Meals.AddRange(
                new Meal { Name = "Kısır", Date = DateTime.Now },
                new Meal { Name = "Mercimek Çorbası", Date = DateTime.Now.AddDays(1) },
                new Meal { Name = "Tavuk Sote", Date = DateTime.Now.AddDays(2) },
                new Meal { Name = "Pasta", Date = DateTime.Now.AddDays(3) }
            );
            context.SaveChanges();
        }

    }
}
        
