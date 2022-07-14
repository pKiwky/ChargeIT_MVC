using ChargeIT.Data;
using ChargeIT.Data.Entities;
using ChargeIT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChargeIT.Controllers {

    public class CarOwnersController : Controller {
        private readonly ApplicationDbContext _applicationDbContext;

        public CarOwnersController(ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index() {
            var carOwnerViewModels = _applicationDbContext.CarOwners
                .Select(co =>
                    new CarOwnerViewModel() {
                        Id = co.Id,
                        Name = co.Name,
                        Email = co.Email
                    }
                ).ToList();

            return View(carOwnerViewModels);
        }

        [HttpGet]
        public IActionResult GetAddCarOwnerModal() {
            var carOwnerViewModel = new CarOwnerViewModel();

            return PartialView("AddCarOwner", carOwnerViewModel);
        }

        [HttpPost]
        public IActionResult AddCarOwner(CarOwnerViewModel carOwnerViewModel) {
            if (ModelState.IsValid == false) {
                return PartialView("AddCarOwner", carOwnerViewModel);
            }

            _applicationDbContext.CarOwners.Add(
                new CarOwnerEntity() {
                    Name = carOwnerViewModel.Name,
                    Email = carOwnerViewModel.Email
                }
            );
            _applicationDbContext.SaveChanges();

            return Json(new {
                message = "The car owners was successfully added.",
            });
        }

        [HttpGet]
        public IActionResult CarOwnerDetails(int id) {
            var carOwner = _applicationDbContext.CarOwners
                .FirstOrDefault(c => c.Id == id && c.IsDeleted == false);

            if (carOwner == null) {
                return Json(new {
                    error = "This car owner it is not valid."
                });
            }

            var carOwnerViewModel = new CarOwnerViewModel() {
                Id = carOwner.Id,
                Name = carOwner.Name,
                Email = carOwner.Email
            };

            var carOwnerCars = _applicationDbContext.Cars
                .Where(c => c.OwnerId == id)
                .Select(c => new CarViewModel() {
                    Id = c.Id,
                    PlateNumber = c.PlateNumber
                })
                .ToList();

            return PartialView("DetailsCarOwner", new CarOwnerDetailsViewModel() {
                CarOwnerViewModel = carOwnerViewModel,
                CarViewModels = carOwnerCars
            });
        }
    }

}