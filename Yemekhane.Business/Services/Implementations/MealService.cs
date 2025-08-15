using AutoMapper;
using Yemekhane.Business.Common;
using Yemekhane.Business.DTOs;
using Yemekhane.Business.Services.Interfaces;
using Yemekhane.Data.Repositories;
using Yemekhane.Entities;


public class MealService : IMealService
{   // <summary>   

    private readonly IMealRepository _mealRepo;
    private readonly IRatingRepository _ratingRepo;
    private readonly IMapper _mapper;
    private readonly INotificationContext _notify;

    public MealService(
       IMealRepository mealRepo,          // <-- Interface'den alıyor
       IRatingRepository ratingRepo,      // <-- Interface """""""""""
       IMapper mapper,
       INotificationContext notify)
    {
        _mealRepo = mealRepo;
        _ratingRepo = ratingRepo;
        _mapper = mapper;
        _notify  = notify;
    }


    public IEnumerable<MealDto> GetWeeklyMenu()
    {
        var today = DateTime.Today;
        var nextWeek = today.AddDays(7);

        var meals = _mealRepo
            .GetAll()
            .Where(m => m.Date >= today && m.Date <= nextWeek)
            .ToList();

        if (meals.Count == 0)
            _notify.Info("WEEKLY_EMPTY", "Önümüzdeki 7 gün için menü bulunamadı.", "Null");

        return _mapper.Map<IEnumerable<MealDto>>(meals);
    }

    public double GetMealAverageRating(int mealId)
    {
        var meal = _mealRepo.GetById(mealId);
        if (meal is null)
        {
            _notify.Error("MEAL_NOT_FOUND", "Yemek bulunamadı.", field: "mealId");
            return 0d;
        }

        var avg = _ratingRepo.GetAverageRatingForMeal(mealId);
        if (avg <= 0)
            _notify.Info("NO_RATINGS", "Bu yemek için henüz puan yok.", field: "mealId");

        return avg;
    }

    public bool RateMeal(int mealId, int userId, int rating, string? comment)
    {
        // 1) Aralık kontrolü
        if (rating < 1 || rating > 5)
        {
            _notify.Error("RATING_OUT_OF_RANGE", "Puan 1-5 arası olmalı.", field: "rating");
            return false;
        }

        // 2) Meal var mı?
        var meal = _mealRepo.GetById(mealId);
        if (meal is null)
        {
            _notify.Error("MEAL_NOT_FOUND", "Puanlanacak yemek bulunamadı.", field: "mealId");
            return false;
        }

        // 3) Aynı user daha önce puanladı mı?
        if (_ratingRepo.HasUserRatedMeal(mealId, userId))
        {
            _notify.Error("RATING_DUP", "Bu öğüne daha önce puan verdiniz.", field: "mealId");
            return false;
        }

        // 4) (Opsiyonel) uzun yorum uyarısı
        if (!string.IsNullOrWhiteSpace(comment) && comment.Length > 500)
            _notify.Warn("COMMENT_TOO_LONG", "Yorum 500 karakteri aşıyor, kısaltın lütfen.", field: "comment");

        var entity = new Rating
        {
            MealId = mealId,
            UserId = userId,
            Score = rating,
            Comment = comment
        };

        _ratingRepo.Add(entity);
        return true;
    }

    public MealDto GetMealById(int id)
    {
        var meal = _mealRepo.GetById(id);
        if (meal is null)
        {
            _notify.Error("MEAL_NOT_FOUND", "Yemek bulunamadı.", field: "id");


            return new MealDto();
        }

        return _mapper.Map<MealDto>(meal);
    }

    public void AddMeal(MealDto dto)
    {
        if (_mealRepo.ExistsByNameAndDate(dto.Name, dto.Date))
        {
            _notify.Error("MEAL_DUPLICATE", "Aynı gün aynı isimde yemek mevcut.", field: "name");
            return;
        }

        var meal = _mapper.Map<Meal>(dto);
        _mealRepo.Add(meal);
    }

    public void UpdateMeal(MealDto dto)
    {
        var meal = _mealRepo.GetById(dto.Id);
        if (meal is null)
        {
            _notify.Error("MEAL_NOT_FOUND", "Güncellenecek yemek bulunamadı.", field: "id");
            return;
        }

        // İsim veya tarih değişiyor mu?
        var willChangeIdentity =
            !string.Equals(meal.Name, dto.Name, StringComparison.OrdinalIgnoreCase) ||
            meal.Date.Date != dto.Date.Date;

        if (willChangeIdentity && _mealRepo.ExistsByNameAndDate(dto.Name, dto.Date))
        {
            _notify.Error("MEAL_DUPLICATE", "Aynı gün aynı isimde yemek mevcut.", field: "name");
            return;
        }

        _mapper.Map(dto, meal);
        _mealRepo.Update(meal);
    }

    public void DeleteMeal(int id)
    {
        var meal = _mealRepo.GetById(id);
        if (meal is null)
        {
            _notify.Error("MEAL_NOT_FOUND", "Silinecek yemek bulunamadı.", field: "id");
            return;
        }
        _mealRepo.Delete(meal.Id);
    }
}
