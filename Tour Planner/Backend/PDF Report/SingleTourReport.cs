using Shared.Models;

namespace Backend.PDF_Report;

using System.IO;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class SingleTourReport
{
    public async Task<KeyValuePair<string, bool>> GenerateTourReportAsync(Tour tour)
    {
        var outputPath = Path.Combine("wwwroot", "report", "single", $"TourReport_{tour.Name}.pdf");
        
        try
        {
            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();
                doc.Add(new Paragraph($"Name: {tour.Name}"));
                doc.Add(new Paragraph($"Description: {tour.TourDescription}"));
                doc.Add(new Paragraph($"From: {tour.From}"));
                doc.Add(new Paragraph($"To: {tour.To}"));
                doc.Add(new Paragraph($"Transportation Type: {tour.TransportationType}"));
                doc.Add(new Paragraph($"Distance: {tour.Distance} km"));
                doc.Add(new Paragraph($"Duration: {tour.Duration}"));
                doc.Add(new Paragraph($"Image: {tour.Image}"));
                doc.Add(new Paragraph(" "));

                doc.Add(new Paragraph("Tour Logs:"));
                foreach (var log in tour.TourLogs)
                {
                    doc.Add(new Paragraph($"Date: {log.DateTime}"));
                    doc.Add(new Paragraph($"Comment: {log.Comment}"));
                    doc.Add(new Paragraph($"Difficulty: {log.Difficulty}"));
                    doc.Add(new Paragraph($"Total Distance: {log.TotalDistance} km"));
                    doc.Add(new Paragraph($"Total Time: {log.TotalTime}"));
                    doc.Add(new Paragraph($"Rating: {log.Rating}"));
                    doc.Add(new Paragraph(" "));
                }

                doc.Close();
            }
        }
        catch (DocumentException)
        {
            return new KeyValuePair<string, bool>("", false);
        }
        catch (IOException ex)
        {
            return new KeyValuePair<string, bool>($"IOException: {ex.Message}", false);
        }
        catch (UnauthorizedAccessException ex)
        {
            return new KeyValuePair<string, bool>($"UnauthorizedAccessException: {ex.Message}", false);
        }
        catch (ArgumentException ex)
        {
            return new KeyValuePair<string, bool>($"ArgumentException: {ex.Message}", false);
        }
        catch (Exception ex)
        {
            return new KeyValuePair<string, bool>($"Unexpected error: {ex.Message}", false);
        }

        return new KeyValuePair<string, bool>(outputPath, true);
    }
}