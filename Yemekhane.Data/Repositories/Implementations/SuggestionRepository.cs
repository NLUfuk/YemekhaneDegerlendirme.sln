namespace Yemekhane.Data.Repositories;

using Yemekhane.Entities;

public class SuggestionRepository : ISuggestionRepository
{
    private readonly AppDbContext _context;
    public SuggestionRepository(AppDbContext context) => _context = context;

    public IQueryable<MealSuggestion> Query()
        => _context.MealSuggestions;

    public IEnumerable<MealSuggestion> GetAll()
        => _context.MealSuggestions.OrderByDescending(s => s.VoteCount).ToList();

    public MealSuggestion? GetByName(string mealName)
        => _context.MealSuggestions.FirstOrDefault(s => s.MealName == mealName);

    public void AddSuggestion(MealSuggestion suggestion)
    {
        _context.MealSuggestions.Add(suggestion);
        _context.SaveChanges();
    }

    public void IncrementVote(MealSuggestion suggestion)
    {
        suggestion.VoteCount += 1;
        _context.MealSuggestions.Update(suggestion);
        _context.SaveChanges();
    }

    public int IncrementVoteByName(string mealName)
    {
        var suggestion = _context.MealSuggestions.FirstOrDefault(s => s.MealName == mealName);
        if (suggestion == null)
            return -1;
        suggestion.VoteCount += 1;
        _context.MealSuggestions.Update(suggestion);
        _context.SaveChanges();
        return suggestion.VoteCount;
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
