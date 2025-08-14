namespace Yemekhane.Business.DTOs
{
    public class SuggestionStatDto
    {
        public string MealName { get; set; } = string.Empty;
        public int VoteCount { get; set; }
        public int? MealId { get; set; }               // eşleşen meal varsa
        public double MealAverageScore { get; set; }     // varsa ortalama, yoksa 0
    }
}
