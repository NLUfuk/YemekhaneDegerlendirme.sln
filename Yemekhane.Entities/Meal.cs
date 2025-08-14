using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yemekhane.Entities
{
    public class Meal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    // id set etmek , sql CRUD op. için zararlı
                                                                 // Cannot insert explicit value for identity column in table 'Meals' when IDENTITY_INSERT is set to OFF

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        // Navigation property
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
