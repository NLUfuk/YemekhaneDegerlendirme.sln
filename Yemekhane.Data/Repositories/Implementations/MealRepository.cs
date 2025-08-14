namespace Yemekhane.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using Yemekhane.Entities;

public class MealRepository : IMealRepository
{
    private readonly AppDbContext _context;
    public MealRepository(AppDbContext context) => _context = context;

    public Meal? GetById(int mealId)
        => _context.Meals
                   .Include(m => m.Ratings)
                   .FirstOrDefault(m => m.Id == mealId);

    public void Add(Meal meal)
    {
        _context.Meals.Add(meal);
        _context.SaveChanges();

    }   

    public IEnumerable<Meal> GetAll()
    {
        return _context.Meals.ToList();
    }

    

    //  Haftalık liste (DateStart--->weekStart)
    public IEnumerable<Meal> GetMealsForWeek(DateTime weekStart)
    {
        var weekEnd = weekStart.AddDays(7);
        return _context.Meals
                       .Include(m => m.Ratings)
                       .Where(m => m.Date >= weekStart && m.Date < weekEnd)
                       .OrderBy(m => m.Date)
                       .ToList();
    }

    //  Güncelle
    public void Update(Meal meal)
    {
        //var existingMeal = _context.Meals.FirstOrDefault(m => m.Id == meal.Id);
        //if (existingMeal == null) return false;

        //// Sadece dolu alanları güncelle
        //if (!string.IsNullOrWhiteSpace(meal.Name))
        //    existingMeal.Name = meal.Name;

        //if (DateTime.Now!=meal.Date)
        //    existingMeal.Date = meal.Date;


        //_context.SaveChanges();
        //return true;
        
   
        _context.Meals.Update(meal);
        _context.SaveChanges();
    }

   


    //  Sil
    public bool Delete(int id)
    {
        var entity = _context.Meals.Find(id);
        if (entity == null) return false;

        _context.Meals.Remove(entity);
        _context.SaveChanges();
        return true;
    }
}
