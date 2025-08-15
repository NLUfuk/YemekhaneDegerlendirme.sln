

namespace Yemekhane.Business.DTOs
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int MealId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}