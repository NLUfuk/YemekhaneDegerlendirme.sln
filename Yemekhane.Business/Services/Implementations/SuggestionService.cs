using Microsoft.EntityFrameworkCore;
using Yemekhane.Business.DTOs;
using Yemekhane.Business.Services.Interfaces;
using Yemekhane.Data;                 // AppDbContext
using Yemekhane.Data.Repositories;    // ISuggestionRepository
using Yemekhane.Entities;

namespace Yemekhane.Business.Services.Implementations
{
    public class SuggestionService : ISuggestionService
    {
        private readonly ISuggestionRepository _sugRepo;
        private readonly AppDbContext _db;   // JOIN için DbContext

        public SuggestionService(ISuggestionRepository sugRepo, AppDbContext db)
        {
            _sugRepo = sugRepo;
            _db = db;
        }

        public IEnumerable<MealSuggestion> GetSuggestions()
            => _sugRepo.GetAll();

        public bool VoteSuggestion(string mealName, int userId)
        {
            if (string.IsNullOrWhiteSpace(mealName))
                throw new ArgumentException("Öneri adı boş olamaz.", nameof(mealName));

            var normalized = mealName.Trim();

            // Basit & güvenli yol: var mı bak → yoksa ekle → varsa artır
            var existing = _sugRepo.GetByName(normalized);
            if (existing == null)
            {
                var newSug = new MealSuggestion { MealName = normalized, VoteCount = 1 };
                _sugRepo.AddSuggestion(newSug);
                _sugRepo.Save();
                return true;
            }
            else
            {
                _sugRepo.IncrementVote(existing);
                _sugRepo.Save();
                return true;
            }
        }

        //  JOIN'li istatistik
        public IEnumerable<SuggestionStatDto> GetSuggestionStats(DateTime? from = null, DateTime? to = null)
        {
            // Tarih filtresi meal'lere uygulanır 
            var mealsQ = _db.Meals.AsQueryable();
            if (from.HasValue) mealsQ = mealsQ.Where(m => m.Date >= from.Value.Date);
            if (to.HasValue) mealsQ = mealsQ.Where(m => m.Date <= to.Value.Date);

            // s LEFT JOIN m (isim eşleşmesi) + ratings ile ortalama
            var query =
                from s in _db.MealSuggestions
                join m in mealsQ on s.MealName equals m.Name into sm
                from m in sm.DefaultIfEmpty() // LEFT
                join r in _db.Ratings on (m != null ? m.Id : 0) equals r.MealId into rgrp
                select new SuggestionStatDto
                {
                    MealName = s.MealName,
                    VoteCount = s.VoteCount,
                    MealId = m != null ? m.Id : (int?)null,
                    MealAverageScore = rgrp.Select(x => (double?)x.Score).Average() ?? 0
                };

            return query.AsNoTracking()
                        .OrderByDescending(x => x.VoteCount)
                        .ThenByDescending(x => x.MealAverageScore)
                        .ToList();
        }
    }
}
