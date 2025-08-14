using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yemekhane.Business.DTOs
{
    public class MealDto
    {
        //public int Id { get; set; }
        //public string Name { get; set; } = string.Empty;
        //public DateTime Date { get; set; }

        public int Id { get; set; }          // Add işlemi için göndermene gerek yok
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        // Sunum için meal içinde atama yok
        public double AverageScore { get; set; }        
        public int RatingsCount { get; set; }
    }
}
