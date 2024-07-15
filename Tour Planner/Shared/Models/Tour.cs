using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class Tour
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string TourDescription { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public TransportationType TransportationType { get; set; }
    public double Distance { get; set; }
    public TimeSpan Duration { get; set; }
    public string Image { get; set; }
    
    public IEnumerable<TourLog> TourLogs { get; set; }
    
    public Tour(string name, string tourDescription, string from, string to, TransportationType transportationType, double distance, TimeSpan duration, string image, IEnumerable<TourLog> tourLogs)
    {
        Name = name;
        TourDescription = tourDescription;
        From = from;
        To = to;
        TransportationType = transportationType;
        Distance = distance;
        Duration = duration;
        Image = image;
        tourLogs = new List<TourLog>();
    }
}