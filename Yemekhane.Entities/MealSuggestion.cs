using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yemekhane.Entities;

public class MealSuggestion
{
    public int Id { get; set; }
    public string MealName { get; set; } = string.Empty; // Name of the istenilen yemek
    public int VoteCount { get; set; }
}
