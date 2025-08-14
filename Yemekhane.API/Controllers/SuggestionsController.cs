using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Yemekhane.Business.DTOs;
using Yemekhane.Business.Services.Interfaces;
using Yemekhane.Entities;

namespace Yemekhane.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuggestionsController : ControllerBase
    {
        private readonly ISuggestionService _svc;
        public SuggestionsController(ISuggestionService svc) => _svc = svc;

        [HttpGet]
        public ActionResult<IEnumerable<MealSuggestion>> All()
            => Ok(_svc.GetSuggestions());

        [HttpPost("vote")]
        public IActionResult Vote([FromBody] string mealName)
        {
            _svc.VoteSuggestion(mealName, userId: 1); // örnek; prod'da claims'ten al
            return NoContent();
        }

        // ✅ İstatistik
        // /api/suggestions/stats?from=2025-08-01&to=2025-08-31
        [HttpGet("stats")]
        public ActionResult<IEnumerable<SuggestionStatDto>> Stats([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
            => Ok(_svc.GetSuggestionStats(from, to));
    }
}
