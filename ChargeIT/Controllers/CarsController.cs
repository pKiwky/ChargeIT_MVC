using ChargeIT.Data;
using ChargeIT.Data.Entities;
using ChargeIT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChargeIT.Controllers {

    public class CarsController : Controller {
        private readonly ApplicationDbContext _applicationDbContext;

        public CarsController(ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index() {
            List<CarViewModel> carViewModels = _applicationDbContext.Cars
                .Where(c => c.IsDeleted == false)
                .Select(c =>
                    new CarViewModel() {
                        Id = c.Id,
                        PlateNumber = c.PlateNumber
                    }
                ).ToList();

            return View(carViewModels);
        }

        [HttpGet]
        public IActionResult GetAddCarModal() {
            var addCarViewModel = new AddCarViewModel();

            addCarViewModel.Owners = _applicationDbContext.CarOwners
                .Select(co => new DropdownViewModel() {
                    Id = co.Id,
                    Value = co.Name
                }
                ).ToList();

            return PartialView("AddCar", addCarViewModel);
        }

        [HttpPost]
        public IActionResult AddCar(AddCarViewModel addCarViewModel) {
            if (ModelState.IsValid == false){
                addCarViewModel.Owners = _applicationDbContext.CarOwners
                    .Select(co => new DropdownViewModel() {
                        Id = co.Id,
                        Value = co.Name
                    }
                ).ToList();

                return PartialView("AddCar", addCarViewModel);
            }

            _applicationDbContext.Cars.Add(
                new CarEntity() {
                    PlateNumber = addCarViewModel.PlateNumber,
                    OwnerId = addCarViewModel.OwnerId,
                }
            );
            _applicationDbContext.SaveChanges();

            return Json(new {
                message = "The car was successfully added.",
            });
        }

        [HttpGet]
        public IActionResult GetEditCarModal(int id) {
            var carEntity = _applicationDbContext.Cars
                .FirstOrDefault(c => c.Id == id && c.IsDeleted == false);

            if (carEntity == null){
                return new EmptyResult();
            }

            var carViewModel = new AddCarViewModel() {
                PlateNumber = carEntity.PlateNumber,
                OwnerId = carEntity.OwnerId
            };
                
            carViewModel.Owners = _applicationDbContext.CarOwners
                .Select(co => new DropdownViewModel() {
                    Id = co.Id,
                    Value = co.Name
                }
            ).ToList();
            
            return PartialView("EditCar", carViewModel);
        }
        
        [HttpPost]
        public IActionResult EditCar(AddCarViewModel addCarViewModel) {
            if (ModelState.IsValid == false) {
                addCarViewModel.Owners = _applicationDbContext.CarOwners
                    .Select(co => new DropdownViewModel() {
                        Id = co.Id,
                        Value = co.Name
                    }
                ).ToList();
                
                return View("EditCar", addCarViewModel);
            }

            var carEntity = _applicationDbContext.Cars
                .FirstOrDefault(c => c.Id == addCarViewModel.Id && c.IsDeleted == false);

            if (carEntity == null) {
                return Json(new {
                    error = "This car it is not valid."
                });
            }

            carEntity.PlateNumber = addCarViewModel.PlateNumber;
            carEntity.OwnerId = addCarViewModel.OwnerId;

            _applicationDbContext.Cars.Update(carEntity);
            _applicationDbContext.SaveChanges();

            return Json(new {
                message = "The car was successfully edited.",
            });
        }

        [HttpDelete]
        public IActionResult DeleteCar(int id) {
            var carEntity = _applicationDbContext.Cars
                .FirstOrDefault(c => c.Id == id && c.IsDeleted == false);

            if (carEntity == null){
                return Json(new {
                    error = "This car was already deleted.",
                });
            }

            _applicationDbContext.Cars.Remove(carEntity);
            _applicationDbContext.SaveChanges();

            return Json(new {
                message = "The car was successfully deleted.",
            });

            return RedirectToAction("Index", "Cars");
        }
        
        [HttpGet]
        public IActionResult CarDetails(int id) {
            var carEntity = _applicationDbContext.Cars
                .Include(c => c.Owner)
                .FirstOrDefault(c => c.Id == id && c.IsDeleted == false);

            if (carEntity == null) {
                return Json(new {
                    error = "This car it is not valid."
                });
            }

            CarViewModel carViewModel = new CarViewModel() {
                Id = carEntity.Id,
                PlateNumber = carEntity.PlateNumber,
                Owner = new CarOwnerViewModel() {
                    Name = carEntity.Owner.Name,
                    Email = carEntity.Owner.Email
                }
            };

            var chargeMachineViewModels = _applicationDbContext.Bookings
                .Include(b => b.ChargeMachine)
                .Where(b => b.CarId == carViewModel.Id)
                .Select(b => new ChargeMachineBookingViewModel() {
                    BookingViewModel = new BookingViewModel() {
                        Date = b.StartTime,
                        Code = b.Code
                    },
                    ChargeMachineViewModel = new ChargeMachineViewModel() {
                        City = b.ChargeMachine.City,
                    }
                })
                .OrderByDescending(b => b.BookingViewModel.Date)
                .ToList();

            return PartialView("DetailsCar", new CarDetailsViewModel() { CarViewModel = carViewModel, ChargeMachineBookingViewModels = chargeMachineViewModels });
        }
    }

}
