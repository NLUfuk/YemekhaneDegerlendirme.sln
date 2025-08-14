using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Yemekhane.Business.DTOs;
using Yemekhane.Business.Services.Interfaces;
using Yemekhane.Data.Repositories;
using Yemekhane.Entities;

public class MealService : IMealService
{
    // Fix ENC0020: Use original field names as in interface/other usages
    private readonly IMealRepository _mealRepo;
    private readonly IRatingRepository _ratingRepo;
    private readonly IMapper _mapper; // AutoMapper

    public MealService(IMealRepository mealRepo, IRatingRepository ratingRepo, IMapper mapper)
    {
        _mealRepo = mealRepo;
        _ratingRepo = ratingRepo;
        _mapper = mapper;
    }

    public IEnumerable<MealDto> GetWeeklyMenu()
    {
        var today = DateTime.Today;
        var nextWeek = today.AddDays(7);
        var meals = _mealRepo
            .GetAll()
            .Where(m => m.Date >= today && m.Date <= nextWeek)
            .ToList();

        return _mapper.Map<IEnumerable<MealDto>>(meals);
    }

    // Fix ENC0047: Do not change method visibility (keep as public)
    public double GetMealAverageRating(int mealId)
    {
        // Use the repository method instead of GetAll()
        var avg = _ratingRepo.GetAverageRatingForMeal(mealId);
        return avg;
    }

    // Fix ENC0033: Do not delete this method
    public bool RateMeal(int mealId, int userId, int rating, string comment)
    {
        // You need a way to check if the user already rated the meal.
        // If IRatingRepository does not provide this, you must add such a method.
        // For now, let's assume you add a method: bool HasUserRatedMeal(int mealId, int userId)
        var alreadyRated = _ratingRepo.HasUserRatedMeal(mealId, userId);

        if (alreadyRated)
            return false;

        var newRating = new Rating
        {
            MealId = mealId,
            UserId = userId,
            Score = rating,
            Comment = comment
            
        };

        _ratingRepo.Add(newRating);

        return true;
    }

    // Fix ENC0033: Do not delete this method
    public MealDto GetMealById(int id)
    {
        var meal = _mealRepo.GetById(id);
        if (meal == null)
            throw new KeyNotFoundException("Yemek bulunamadı.");

        return _mapper.Map<MealDto>(meal);
    }

    // Fix ENC0033: Do not delete this method
    public void AddMeal(MealDto dto)
    {
        var meal = _mapper.Map<Meal>(dto);
        _mealRepo.Add(meal);
    }

    // Fix ENC0033: Do not delete this method
    public void UpdateMeal(MealDto dto)
    {
        var meal = _mealRepo.GetById(dto.Id);
        if (meal is null)
            throw new KeyNotFoundException("Güncellenecek yemek bulunamadı.");

        _mapper.Map(dto, meal); // Mevcut entity'ye DTO değerlerini uygula
        _mealRepo.Update(meal);
    }

    // Fix ENC0033: Do not delete this method
    public void DeleteMeal(int id)
    {
        var meal = _mealRepo.GetById(id);
        if (meal is null)
            throw new KeyNotFoundException("Silinecek yemek bulunamadı.");

        _mealRepo.Delete(meal.Id);
    }
}
