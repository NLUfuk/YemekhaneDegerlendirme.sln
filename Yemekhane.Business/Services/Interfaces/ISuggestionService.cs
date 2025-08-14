using System;
using System.Collections.Generic;
using Yemekhane.Business.DTOs;
using Yemekhane.Entities;

namespace Yemekhane.Business.Services.Interfaces
{
    public interface ISuggestionService
    {
        IEnumerable<MealSuggestion> GetSuggestions();
        bool VoteSuggestion(string mealName, int userId);

        // ✅ Yeni: İstatistik üret
        IEnumerable<SuggestionStatDto> GetSuggestionStats(DateTime? from = null, DateTime? to = null);
    }
}
