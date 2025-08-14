namespace Yemekhane.Data.Repositories;

using Yemekhane.Entities;

public class RatingRepository : IRatingRepository
{
    private readonly AppDbContext _context;
    public RatingRepository(AppDbContext context) => _context = context;

    public void Add(Rating rating)
    {
        _context.Ratings.Add(rating);
        _context.SaveChanges();
    }

    public double GetAverageRatingForMeal(int mealId)
        => _context.Ratings
            .Where(r => r.MealId == mealId)
            .Select(r => (double)r.Score)
            .DefaultIfEmpty()
            .Average();

    public bool HasUserRatedMeal(int mealId, int userId)
        => _context.Ratings.Any(r => r.MealId == mealId && r.UserId == userId);
}
