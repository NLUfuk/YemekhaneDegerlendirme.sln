namespace Yemekhane.Data.Repositories;

using Yemekhane.Entities;

public interface IRatingRepository
{
    void Add(Rating rating);
    double GetAverageRatingForMeal(int mealId);
    bool HasUserRatedMeal(int mealId, int userId);
}
