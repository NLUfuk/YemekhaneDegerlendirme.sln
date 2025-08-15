using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yemekhane.Business.Common;
using Yemekhane.Business.DTOs;
using Yemekhane.Business.Services.Interfaces;


namespace Yemekhane.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MealsController(IMealService meals, INotificationContext notify) : ControllerBase
    {
        private readonly IMealService _meals = meals;
        private readonly INotificationContext _notify = notify;

        // Haftalık menü
        [HttpGet("weekly")]
        public IActionResult Weekly()
        {
            var data = _meals.GetWeeklyMenu();
            var response = new
            {
                success = !_notify.HasErrors,
                data,
                notifications = _notify.All.Select(n => new
                {
                    type = n.Type.ToString().ToLower(),
                    code = n.Code,
                    message = n.Message,
                    //field = n.Field
                })
            };
            return Ok(response);
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

        [HttpPost("{mealId}/rate")]
        public IActionResult Rate(int mealId, [FromBody] RatingDto dto)
        {
            var ok = _meals.RateMeal(mealId, /*UserId()*/ dto.UserId, dto.Score, dto.Comment);
            var response = new
            {
                success = ok && !_notify.HasErrors,
                data = new { ok },
                notifications = _notify.All.Select(n => new
                {
                    type = n.Type.ToString().ToLower(),
                    code = n.Code,
                    message = n.Message,

                })
            };
            return (ok && !_notify.HasErrors) ? Ok(response) : UnprocessableEntity(response);
        }


    }

    public record RateMealDto(int Score);
}
