// Add this method to the IMealRepository interface
Meal? GetMealWithRatings(int mealId);
// Implement the new method in your MealRepository class
public Meal? GetMealWithRatings(int mealId)
{
    // Example implementation, adjust according to your ORM (e.g., Entity Framework)
    // Assuming you have a DbContext named _context and Meal has a Ratings navigation property
    return _context.Meals
        .Include(m => m.Ratings)
        .FirstOrDefault(m => m.Id == mealId);
}
