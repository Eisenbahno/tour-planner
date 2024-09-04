using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models;

public class TourLog
{
        [Key] // Marks IdTourLog as primary key
        public int IdTourLog { get; set; }

        public DateTime DateTime { get; set; }
        public string? Comment { get; set; }
        public int Difficulty { get; set; } // Assuming difficulty is an integer scale
        public double TotalDistance { get; set; } // Assuming total distance is in kilometers or miles
        public TimeSpan TotalTime { get; set; } // TimeSpan to represent total time taken
        
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } // Assuming rating is an integer scale

        [ForeignKey("Tour")] // ForeignKey attribute, pointing to the Tour class
        public int TourId { get; set; } // Foreign key property

        public Tour Tour { get; set; } // Navigation property

}