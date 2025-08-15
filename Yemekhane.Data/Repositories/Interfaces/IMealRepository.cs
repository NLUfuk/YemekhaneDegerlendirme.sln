namespace Yemekhane.Data.Repositories;

using Yemekhane.Entities;

public interface IMealRepository
{
    Meal? GetById(int mealId);
    void Add(Meal meal);

    
    IEnumerable<Meal> GetAll(); // Tüm yemekleri getir  
    IEnumerable<Meal> GetMealsForWeek(DateTime weekStart); // Pazartesi-bazlı liste
    void Update(Meal meal);
    bool Delete(int id);

    bool ExistsByNameAndDate(string name, DateTime date);
}
