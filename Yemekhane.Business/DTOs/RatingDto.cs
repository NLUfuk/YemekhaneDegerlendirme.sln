using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yemekhane.Entities;

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