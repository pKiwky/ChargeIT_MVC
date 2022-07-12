using ChargeIT.Data;
using ChargeIT.Data.Entities;
using ChargeIT.Models;
using Microsoft.AspNetCore.Mvc;

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
            if (ModelState.IsValid == false) {
                addCarViewModel.Owners = _applicationDbContext.CarOwners
                    .Select(co => new DropdownViewModel() {
                            Id = co.Id,
                            Value = co.Name
                        }
                    ).ToList();

                return View("AddCar", addCarViewModel);
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
    }

}