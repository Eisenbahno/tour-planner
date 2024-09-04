using iTextSharp.text;
using iTextSharp.text.pdf;
using Shared.Models;

namespace Backend.PDF_Report;

public class SummarizedReport
{
    public async Task<bool> GenerateSummarizedTourReportAsync(List<Tour> tours)
    {
        var outputPath = Path.Combine("wwwroot", "pdfs", $"SummarizedTourReport_{DateTime.Now:yyyyMMddHHmmss}.pdf");

        try
        {
            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Add title
                doc.Add(new Paragraph("Summarized Tour Report"));
                doc.Add(new Paragraph($"Date: {DateTime.Now:MMMM dd, yyyy}"));
                doc.Add(new Paragraph(" "));

                foreach (var tour in tours)
                {
                    doc.Add(new Paragraph($"Name: {tour.Name}"));
                    doc.Add(new Paragraph($"Description: {tour.TourDescription}"));
                    doc.Add(new Paragraph($"From: {tour.From}"));
                    doc.Add(new Paragraph($"To: {tour.To}"));
                    doc.Add(new Paragraph($"Transportation Type: {tour.TransportationType}"));
                    doc.Add(new Paragraph($"Distance: {tour.Distance} km"));
                    doc.Add(new Paragraph($"Duration: {tour.Duration}"));
                    doc.Add(new Paragraph($"Image: {tour.Image}"));
                    doc.Add(new Paragraph(" "));

                    // Add tour logs
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

                    doc.Add(new Paragraph(" "));
                    doc.Add(new Paragraph("========================================"));
                    doc.Add(new Paragraph(" "));
                }

                doc.Close();
            }

            return true;
        }
        catch (DocumentException ex)
        {
            Console.WriteLine($"DocumentException: {ex.Message}");
            return false;
        }
        catch (IOException ex)
        {
            Console.WriteLine($"IOException: {ex.Message}");
            return false;
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"UnauthorizedAccessException: {ex.Message}");
            return false;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"ArgumentException: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return false;
        }
    }
}