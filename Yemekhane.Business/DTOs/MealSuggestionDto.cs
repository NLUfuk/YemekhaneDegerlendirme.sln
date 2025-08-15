
namespace Yemekhane.Business.DTOs
{
    public class MealSuggestionDto
    {
        public string MealName { get; set; } = string.Empty; // Name of the istenilen yemek
        public int VoteCount { get; set; }
        public int Rating { get; set; }           
        public string? Comment { get; set; }
    }
}
