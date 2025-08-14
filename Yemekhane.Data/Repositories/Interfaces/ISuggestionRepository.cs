namespace Yemekhane.Data.Repositories;

using Yemekhane.Entities;

public interface ISuggestionRepository
{
    //IEnumerable<MealSuggestion> GetAll();
    //MealSuggestion? GetByName(string mealName);
    //void AddSuggestion(MealSuggestion suggestion);
    //void IncrementVote(MealSuggestion suggestion);
    IQueryable<MealSuggestion> Query();        // LINQ için
    IEnumerable<MealSuggestion> GetAll();
    MealSuggestion? GetByName(string mealName);
    void AddSuggestion(MealSuggestion suggestion);
    void IncrementVote(MealSuggestion suggestion); // klasik Fetch→++→SaveChanges
    int IncrementVoteByName(string mealName);      // EF ExecuteUpdate ile atomik ++ (opsiyonel)
    void Save();
}
