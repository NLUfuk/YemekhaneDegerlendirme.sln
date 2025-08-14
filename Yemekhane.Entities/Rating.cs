using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Yemekhane.Entities;

namespace Yemekhane.Entities
{
    public class Rating
    {
        [Key]
        public int Id { get; set; } // Identity

        [Range(1, 5)]
        public int Score { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //public DateTime UpdatedAt { get; set; }

        // FK'ler
        public int MealId { get; set; }
        public int UserId { get; set; } // Basit örnek; ASP.NET Identity ile maplenebilir

        //// Nav
        public Meal? Meal { get; set; }
    }
}
