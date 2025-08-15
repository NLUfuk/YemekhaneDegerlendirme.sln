using Yemekhane.Business.DTOs;

namespace Yemekhane.Business.Services.Interfaces
{
    public interface IMealService
    {
        IEnumerable<MealDto> GetWeeklyMenu();
        double GetMealAverageRating(int mealId);
        bool RateMeal(int mealId, int userId, int rating, string comment);

        MealDto GetMealById(int id);             //R
        void AddMeal(MealDto dto);               //C
        void UpdateMeal(MealDto dto);             //U
        void DeleteMeal(int id);                  //D




        

    }
}
