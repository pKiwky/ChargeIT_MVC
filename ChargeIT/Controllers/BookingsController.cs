using ChargeIT.Data;
using ChargeIT.Data.Entities;
using ChargeIT.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChargeIT.Controllers {

    public class BookingsController : Controller {
        private const int TotalAvailableHoursInDay = 24;

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly List<int> _totalAvailableHours;
        
        public BookingsController(ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
            
            _totalAvailableHours = new List<int>();
            for (var hour = 0; hour < TotalAvailableHoursInDay; hour++){
                _totalAvailableHours.Add(hour);
            }
        }
        
        public IActionResult Index() {
            List<DropdownViewModel> chargeMachinesViewModel = _applicationDbContext.ChargeMachines.Select(cm =>
                new DropdownViewModel() {
                    Id = cm.Id,
                    Value = $"{cm.City} {cm.Street}"
                }
            ).ToList();

            List<DropdownViewModel> carsViewModels = _applicationDbContext.Cars.Select(c =>
                new DropdownViewModel() {
                    Id = c.Id,
                    Value = c.PlateNumber
                }
            ).ToList();

            return View("Index", new BookingViewModel() { Cars = carsViewModels, ChargeMachines = chargeMachinesViewModel });
        }
        
        [HttpGet]
        public ActionResult<List<int>> GetAvailableIntervals(int chargeMachineId, DateTime date) {
            List<int> notAvailableHours = _applicationDbContext.Bookings.Where(b =>
                b.ChargeMachineId == chargeMachineId && b.StartTime >= date && b.StartTime <= date.AddHours(23).AddMinutes(59).AddSeconds(59)
            ).Select(b => b.StartTime.Hour).ToList();

            List<int> availableHours = _totalAvailableHours.Except(notAvailableHours).ToList();

            return availableHours;
        }

        [HttpPost]
        public IActionResult AddBooking(BookingViewModel bookingViewModel) {
            if (ModelState.IsValid) {
                DateTime startTime = bookingViewModel.Date.Value.AddHours(bookingViewModel.IntervalHour);

                if (startTime < DateTime.Now.Subtract(TimeSpan.FromHours(1))){
                    ModelState.AddModelError(nameof(BookingViewModel.Date), "Date time is in past.");
                }

                if (_applicationDbContext.Bookings
                    .Any(b => b.ChargeMachineId == bookingViewModel.ChargeMachineId && b.StartTime == startTime)){
                    ModelState.AddModelError(nameof(BookingViewModel.IntervalHour), "Interval already used for another booking.");
                }

                if (_applicationDbContext.Bookings
                    .Any(b => b.CarId == bookingViewModel.CarId && b.StartTime == startTime)) {
                    ModelState.AddModelError(nameof(BookingViewModel.CarId), "Car already used for another booking.");
                }

                if (ModelState.IsValid) {
                    _applicationDbContext.Bookings.Add(
                        new BookingEntity() {
                            Code = Guid.NewGuid(),
                            StartTime = startTime,
                            EndTime = startTime.AddMinutes(59).AddSeconds(59),
                            CarId = bookingViewModel.CarId,
                            ChargeMachineId = bookingViewModel.ChargeMachineId
                        }
                    );
                    _applicationDbContext.SaveChanges();

                    return Json(new {
                        message = "The bookings was successfully created.",
                    });
                }
            }

            bookingViewModel.ChargeMachines = _applicationDbContext.ChargeMachines.Select(cm =>
                new DropdownViewModel() {
                    Id = cm.Id,
                    Value = $"{cm.City} {cm.Street}"
                }
            ).ToList();

            bookingViewModel.Cars = _applicationDbContext.Cars.Select(c =>
                new DropdownViewModel() {
                    Id = c.Id,
                    Value = c.PlateNumber
                }
            ).ToList();
            
            return View("Index", bookingViewModel);
        }
    }

}
