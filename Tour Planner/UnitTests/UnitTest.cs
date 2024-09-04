using Backend.DbContext;
using Backend.PDF_Report;
using Backend.Repository;
using Moq;
using Shared.Models;
using Shared.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Legacy;

namespace UnitTests
{
    [TestFixture]
    public class TourPlannerTests
    {
        private TourRepository _repository;
        private Mock<ToursDbContext> _mockContext;
        private Mock<DbSet<Tour>> _mockDbSet;
        private SingleTourReport _singleTourReport;
        private SummarizedReport _summarizedReport;
        private OpenRouteService _service;

        [SetUp]
        public void Setup()
        {
            _mockDbSet = new Mock<DbSet<Tour>>();
            _mockContext = new Mock<ToursDbContext>();
            _mockContext.Setup(c => c.Tours).Returns(_mockDbSet.Object);
            _repository = new TourRepository(_mockContext.Object);

            _singleTourReport = new SingleTourReport();
            _summarizedReport = new SummarizedReport();


            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            string? apiKey = configuration["OpenRouteService:ApiKey"];
            _service = new OpenRouteService(new HttpClient(), apiKey);
        }

        [Test]
        public void AddTour_Should_Add_New_Tour()
        {
            var newTour = new Tour { Id = 1, Name = "New Tour" };
            _repository.AddTourAsync(newTour);
            _mockDbSet.Verify(m => m.Add(It.IsAny<Tour>()), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void GetTourById_Should_Return_Correct_Tour()
        {
            var tour = new Tour { Id = 1, Name = "Test Tour" };
            _mockDbSet.Setup(m => m.Find(It.IsAny<int>())).Returns(tour);

            var result = _repository.ByIdAsync(1);

            Assert.Equals(tour, result);
        }

        [Test]
        public void GetAllTours_Should_Return_All_Tours()
        {
            var tours = new List<Tour>
            {
                new Tour { Id = 1, Name = "Tour 1" },
                new Tour { Id = 2, Name = "Tour 2" }
            }.AsQueryable();

            _mockDbSet.As<IQueryable<Tour>>().Setup(m => m.Provider).Returns(tours.Provider);
            _mockDbSet.As<IQueryable<Tour>>().Setup(m => m.Expression).Returns(tours.Expression);
            _mockDbSet.As<IQueryable<Tour>>().Setup(m => m.ElementType).Returns(tours.ElementType);
            _mockDbSet.As<IQueryable<Tour>>().Setup(m => m.GetEnumerator()).Returns(tours.GetEnumerator());

            var result = _repository.GetAllToursWithLogsAsync();

            Assert.Equals(2, result.Result.Count());
        }

        [Test]
        public void UpdateTour_Should_Modify_Existing_Tour()
        {
            var existingTour = new Tour { Id = 1, Name = "Existing Tour" };
            _mockDbSet.Setup(m => m.Find(It.IsAny<int>())).Returns(existingTour);

            existingTour.Name = "Updated Tour";
            _repository.AddTourAsync(existingTour);

            Assert.Equals("Updated Tour", existingTour.Name);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void DeleteTour_Should_Remove_Tour()
        {
            var tour = new Tour { Id = 1, Name = "Tour to be deleted" };
            _mockDbSet.Setup(m => m.Find(It.IsAny<int>())).Returns(tour);

            _repository.RemoveTourAsync(1);

            _mockDbSet.Verify(m => m.Remove(It.IsAny<Tour>()), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void AddTour_Should_Throw_Exception_For_Duplicate_Tour()
        {
            var duplicateTour = new Tour { Id = 1, Name = "Duplicate Tour" };
            _mockDbSet.Setup(m => m.Add(It.IsAny<Tour>())).Throws(new DbUpdateException());

            Assert.Throws<DbUpdateException>(() => _repository.AddTourAsync(duplicateTour));
            _mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Test]
        public void GetTourById_Should_Return_Null_For_Invalid_Id()
        {
            _mockDbSet.Setup(m => m.Find(It.IsAny<int>())).Returns((Tour)null);

            var result = _repository.ByIdAsync(999);

            Assert.Equals(null, result);
        }

        [Test]
        public void UpdateTour_Should_Throw_Exception_When_Tour_Not_Found()
        {
            var nonExistentTour = new Tour { Id = 999, Name = "Non-Existent Tour" };
            _mockDbSet.Setup(m => m.Find(It.IsAny<int>())).Returns((Tour)null);

            Assert.Throws<InvalidOperationException>(() => _repository.AddTourAsync(nonExistentTour));
            _mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Test]
        public void CanInsertTourIntoDatabase()
        {
            var options = new DbContextOptionsBuilder<ToursDbContext>()
                .UseInMemoryDatabase(databaseName: "ToursTestDb")
                .Options;

            using (var context = new ToursDbContext(options))
            {
                var tour = new Tour { Name = "Sample Tour" };
                context.Tours.Add(tour);
                context.SaveChanges();
            }

            using (var context = new ToursDbContext(options))
            {
                Assert.Equals(1, context.Tours.Count());
                Assert.Equals("Sample Tour", context.Tours.Single().Name);
            }
        }

        [Test]
        public void SingleTourReport_Should_Contain_Tour_Details()
        {
            var tour = new Tour { Name = "Test Tour", TourDescription = "Test Description" };
            var result = _singleTourReport.GenerateTourReportAsync(tour);

            Assert.Equals(result.Result.Key, "Test Tour");
            Assert.Equals(result.Result, "Test Description");
        }

        [Test]
        public void SummarizedReport_Should_Contain_All_Tours()
        {
            var tours = new List<Tour>
            {
                new Tour { Name = "Tour 1" },
                new Tour { Name = "Tour 2" }
            };
            var result = _summarizedReport.GenerateSummarizedTourReportAsync(tours);

            Assert.Equals(result.Result, true);
            Assert.Equals(result.Result, false);
        }

        [Test]
        public void SummarizedReport_Should_Handle_Empty_Tour_List()
        {
            var tours = new List<Tour>();
            var result = _summarizedReport.GenerateSummarizedTourReportAsync(tours);

            Assert.Equals(result.Result, false);
        }

        [Test]
        public void SingleTourReport_Should_Contain_Formatted_Tour_Details()
        {
            var tour = new Tour { Name = "Test Tour", TourDescription = "Test Description" };
            var result = _singleTourReport.GenerateTourReportAsync(tour);

            StringAssert.Contains("<h1>Test Tour</h1>", result.Result.Key);
            StringAssert.Contains("<p>Test Description</p>", result.Result.Key);
        }

        [Test]
        public void CalculateRoute_Should_Return_Valid_Response()
        {
            var startLat = 48.239240;
            var startLng = 16.377115;
            var endLat = 48.240327;
            var endLng = 16.409728;
            var result = _service.GetRouteAsync(startLat, startLng, endLat, endLng);

            ClassicAssert.Null(result);
            Assert.Equals(result, "Route from Point A to Point B");
        }

        [Test]
        public void CalculateRoute_Should_Handle_Error_Response()
        {
            var startLat = 48.239240;
            var startLng = 16.377115;
            var endLat = 48602869328632;
            var endLng = 8534853474365;
            var result = _service.GetRouteAsync(startLat, startLng, endLat, endLng);

            Assert.Equals(result, "Error");
        }

        [Test]
        public void CalculateRoute_Should_Throw_Exception_For_Invalid_Input()
        {
            var startLat = 48.239240;
            var startLng = 16.377115;
            var endLat = 4860286932632;
            var endLng = 8534853474365;

            Assert.Throws<ArgumentException>(() => _service.GetRouteAsync(startLat, startLng, endLat, endLng));
        }

        [Test]
        public void CalculateRoute_Should_Handle_Api_Error_Response_Gracefully()
        {
            var startLat = 63426326326;
            var startLng = 342563462363;
            var endLat = 4860286932632;
            var endLng = 8534853474365;

            _service.SimulateApiError(true);

            var result = _service.GetRouteAsync(startLat, startLng, endLat, endLng);

            Assert.Equals(result, "API Error");
            _service.SimulateApiError(false);
        }

        [Test]
        public void CalculateRoute_Should_Return_Valid_Data_For_Known_Points()
        {
            var startLat = 0;
            var startLng = 16.377115;
            var endLat = 48.240327;
            var endLng = 16.409728;

            var result = _service.GetRouteAsync(startLat, startLng, endLat, endLng);

            Assert.Equals(result, "Known Start Point");
            Assert.Equals(result, "Known End Point");
        }

        [Test]
        public void TourLog_Should_Have_Valid_Properties()
        {
            var log = new TourLog { IdTourLog = 1, Comment = "Great Tour" };
            Assert.Equals(1, log.IdTourLog);
            Assert.Equals("Great Tour", log.Comment);
        }

        [Test]
        public void Tour_Should_Have_Valid_Properties()
        {
            var tour = new Tour { Id = 1, Name = "Test Tour", TourDescription = "A test tour description" };
            Assert.Equals(1, tour.Id);
            Assert.Equals("Test Tour", tour.Name);
            Assert.Equals("A test tour description", tour.TourDescription);
        }
    }
}