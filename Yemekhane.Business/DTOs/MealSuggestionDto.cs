using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yemekhane.Business.DTOs
{
    public class MealSuggestionDto
    {
        public string MealName { get; set; } = string.Empty; // Name of the istenilen yemek
        public int VoteCount { get; set; }
        public int Rating { get; set; }            // entity de yok burda neişin var 
        public string? Comment { get; set; }
    }
}
