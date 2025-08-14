using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Yemekhane.Business.DTOs;
using Yemekhane.Business.Services.Interfaces;

namespace Yemekhane.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MealsController : ControllerBase
    {
        private readonly IMealService _meals;
        public MealsController(IMealService meals) => _meals = meals;

        // Haftalık menü
        [HttpGet("weekly")]
        public IActionResult Weekly()
        {
            var list = _meals.GetWeeklyMenu()
                .Select(m => new {
                    m.Id,
                    m.Name,
                    m.Date,
                    AverageRating = _meals.GetMealAverageRating(m.Id)
                });
            return Ok(list);
        }

        // Tek yemek detayı
        [HttpGet("{id}")]
        public IActionResult GetMeal(int id)
        {
            var meal = _meals.GetMealById(id);
            if (meal == null) return NotFound();
            return Ok(meal);
        }

        // Yemek ekleme (şimdilik tüm kullanıcılar, sonradan [Authorize(Roles = "Admin")] eklenebilir)
        [HttpPost]
        public IActionResult CreateMeal(MealDto meal)
        {
            _meals.AddMeal(meal);
            return CreatedAtAction(nameof(GetMeal), new { id = meal.Id }, meal);
        }

        // Yemek güncelleme
        [HttpPut("{id}")]
        public IActionResult UpdateMeal(int id, MealDto dto)
        {
            if (id != dto.Id) return BadRequest("Id uyuşmuyor!");

            var existingMeal = _meals.GetMealById(id);
            if (existingMeal == null) return NotFound();

            _meals.UpdateMeal(dto);
            return NoContent();
        }


        // Yemek silme
        [HttpDelete("{id}")]
        public IActionResult DeleteMeal(int id)
        {
            _meals.DeleteMeal(id); // <-- Just call the method, do not assign to a variable
            return NoContent();
        }


        [HttpPost("{id:int}/rate")]
        public IActionResult Rate(int id, [FromBody] RateMealDto dto)   // body yerine dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var ok = _meals.RateMeal(id, userId, dto.Score, string.Empty);
            if (!ok) return Conflict("Bu kullanıcı bu yemeği zaten oylamış.");
            return NoContent();
        }

        //// Yemek puanlama
        //[HttpPost("{id}/rate")]
        //public IActionResult Rate(int id, RateMealDto dto)
        //{
        //    int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        //    if (!_meals.RateMeal(id, userId, dto.Score, string.Empty)) // Pass an empty string for the required 'comment' parameter
        //        return BadRequest("Puan 1-5 olmalı.");

        //    var avg = _meals.GetMealAverageRating(id);
        //    return Ok(new { MealId = id, NewAverageRating = avg });
        //}

        //[HttpPost]                                     //rating create ihtiyaç var mı
        //public IActionResult Create([FromBody] RatingCreateDto dto)
        //{
        //    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        //    var ok = _service.RateMeal(dto.MealId, userId, dto.Rating, dto.Comment);
        //    if (!ok) return Conflict("Bu kullanıcı bu yemeği zaten oylamış.");
        //    return NoContent();
        //}

    }

    public record RateMealDto(int Score);
}
