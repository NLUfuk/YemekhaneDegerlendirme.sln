using System;
using System.Security.Cryptography;
using System.Text;
using Yemekhane.Business.Services.Interfaces;
using Yemekhane.Data.Repositories;
using Yemekhane.Entities;

namespace Yemekhane.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        // Şifre hashing için SHA256 kullanacağız
        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public User Register(string username, string password)
        {
            // Basit kontrol: aynı kullanıcı adı var mı?
            // (IUserRepository'e bir Exists metodu eklenebilirdi. Burada GetByCredentials ile password uyuşmasa da aynı user name kontrolüne uygun değil.
            // Basitlik için şimdilik doğrudan eklemeye çalışacağız.)
            string hash = HashPassword(password);
            var newUser = new User { UserName = username, PasswordHash = hash };
            _userRepo.Add(newUser);
            // Not: SaveChanges çağrısını burada yapmayacağız, API katmanında veya dışarıda yapmak mantıklı olabilir.
            return newUser;
        }

        public User? Authenticate(string username, string password)
        {
            string hash = HashPassword(password);
            return _userRepo.GetByCredentials(username, hash);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            // Hex string olarak SHA256 hash
        }
    }

    //public class MealService : IMealService
    //{
    //    private readonly IMealRepository _mealRepo;
    //    private readonly IRatingRepository _ratingRepo;
    //    public MealService(IMealRepository mealRepo, IRatingRepository ratingRepo)
    //    {
    //        _mealRepo = mealRepo;
    //        _ratingRepo = ratingRepo;
    //    }
    //    public IEnumerable<Meal> GetWeeklyMenu()
    //    {
    //        // Bugünü baz alarak haftalık menüyü getir
    //        DateTime today = DateTime.Today;
    //        var meals = _mealRepo.GetMealsForWeek(today);
    //        return meals;
    //    }
    //    public double GetMealAverageRating(int mealId)
    //    {
    //        return _ratingRepo.GetAverageRatingForMeal(mealId);
    //    }
    //    public bool RateMeal(int mealId, int userId, int score)
    //    {
    //        // Basit kural: score 1-5 arası olmalı
    //        if (score < 1 || score > 5) return false;
    //        // Kullanıcı bu yemeği zaten puanladı mı? (Ek kural koymak istersek burada bakarız, şimdilik izin veriyoruz veya üstüne yazmıyoruz)
    //        // Rating kaydı ekle
    //        var rating = new Rating { MealId = mealId, UserId = userId, Score = score };
    //        _ratingRepo.Add(rating);
    //        return true;
    //    }
    //}

    //public class SuggestionService : ISuggestionService
    //{
    //    private readonly ISuggestionRepository _sugRepo;
    //    public SuggestionService(ISuggestionRepository sugRepo)
    //    {
    //        _sugRepo = sugRepo;
    //    }
    //    public IEnumerable<MealSuggestion> GetSuggestions()
    //    {
    //        return _sugRepo.GetAll();
    //    }
    //    public bool VoteSuggestion(string mealName, int userId)
    //    {
    //        // (userId şu an kullanılmıyor çünkü bir kullanıcıya birden fazla oy engelleme yapılmadı. 
    //        // Geliştirme: Kullanıcı bazlı oy takibi eklenecekse burada kontrol gerekir.)
    //        var existing = _sugRepo.GetByName(mealName);
    //        if (existing == null)
    //        {
    //            // Yeni öneri ekle
    //            var newSug = new MealSuggestion { MealName = mealName, VoteCount = 1 };
    //            _sugRepo.AddSuggestion(newSug);
    //        }
    //        else
    //        {
    //            // Oy sayısını artır
    //            _sugRepo.IncrementVote(existing);
    //        }
    //        return true;
    //    }
    //}
}
