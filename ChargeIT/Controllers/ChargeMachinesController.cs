using ChargeIT.Data;
using ChargeIT.Data.Entities;
using ChargeIT.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChargeIT.Controllers {

    public class ChargeMachinesController : Controller {
        private readonly ApplicationDbContext _applicationDbContext;

        public ChargeMachinesController(ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index() {
            var chargeMachineViewModels = _applicationDbContext.ChargeMachines
                .Where(cm => cm.IsDeleted == false)
                .Select(cm =>
                    new ChargeMachineViewModel() {
                        Id = cm.Id,
                        City = cm.City,
                        Code = cm.Code,
                        Latitude = cm.Latitude,
                        Longitude = cm.Longitude,
                        Number = cm.Number,
                        Street = cm.Street
                    }
                ).ToList();

            return View(chargeMachineViewModels);
        }

        [HttpGet]
        public IActionResult GetAddStationModal() {
            return PartialView("AddStation");
        }

        [HttpPost]
        public IActionResult AddStation(ChargeMachineViewModel chargeMachineViewModel) {
            if (ModelState.IsValid == false) {
                return PartialView("AddStation", chargeMachineViewModel);
            }

            _applicationDbContext.ChargeMachines.Add(
                new ChargeMachineEntity() {
                    Code = Guid.NewGuid(),
                    City = chargeMachineViewModel.City,
                    Latitude = chargeMachineViewModel.Latitude.Value,
                    Longitude = chargeMachineViewModel.Longitude.Value,
                    Number = chargeMachineViewModel.Number,
                    Street = chargeMachineViewModel.Street
                }
            );
            _applicationDbContext.SaveChanges();

            return Json(new {
                message = "The station was successfully added.",
            });
        }

        [HttpGet]
        public IActionResult GetEditStationModal(int id) {
            var chargeMachineEntity = _applicationDbContext.ChargeMachines
                .FirstOrDefault(cm => cm.Id == id && cm.IsDeleted == false);

            if (chargeMachineEntity == null) {
                return new EmptyResult();
            }

            return PartialView("EditStation", new ChargeMachineViewModel() {
                Id = chargeMachineEntity.Id,
                Code = chargeMachineEntity.Code,
                City = chargeMachineEntity.City,
                Latitude = chargeMachineEntity.Latitude,
                Longitude = chargeMachineEntity.Longitude,
                Number = chargeMachineEntity.Number,
                Street = chargeMachineEntity.Street
            });
        }

        [HttpPost]
        public IActionResult EditStation(ChargeMachineViewModel chargeMachineViewModel) {
            if (ModelState.IsValid == false) {
                return View("EditStation", chargeMachineViewModel);
            }

            var chargeMachineEntity = _applicationDbContext.ChargeMachines
                .FirstOrDefault(cm => cm.Id == chargeMachineViewModel.Id && cm.IsDeleted == false);

            if (chargeMachineEntity == null) {
                return Json(new {
                    error = "This station it is not valid."
                });
            }

            chargeMachineEntity.City = chargeMachineViewModel.City;
            chargeMachineEntity.Latitude = chargeMachineViewModel.Latitude.Value;
            chargeMachineEntity.Longitude = chargeMachineViewModel.Longitude.Value;
            chargeMachineEntity.Number = chargeMachineViewModel.Number;
            chargeMachineEntity.Street = chargeMachineViewModel.Street;

            _applicationDbContext.ChargeMachines.Update(chargeMachineEntity);
            _applicationDbContext.SaveChanges();

            return Json(new {
                message = "The station was successfully edited.",
            });
        }

        [HttpDelete]
        public IActionResult DeleteStation(int id) {
            var chargeMachineEntity = _applicationDbContext.ChargeMachines
                .FirstOrDefault(cm => cm.Id == id && cm.IsDeleted == false);

            if (chargeMachineEntity == null) {
                return Json(new {
                    error = "This station was already deleted.",
                });
            }

            _applicationDbContext.ChargeMachines.Remove(chargeMachineEntity);
            _applicationDbContext.SaveChanges();

            return Json(new {
                message = "The station was successfully deleted.",
            });
        }

        [HttpGet]
        public IActionResult DetailsStation(int id) {
            var chargeMachineEntity = _applicationDbContext.ChargeMachines
                .FirstOrDefault(cm => cm.Id == id && cm.IsDeleted == false);

            if (chargeMachineEntity == null) {
                return Json(new {
                    error = "This station it is not valid."
                });
            }

            return PartialView("DetailsStation", new ChargeMachineViewModel() {
                Id = chargeMachineEntity.Id,
                Code = chargeMachineEntity.Code,
                City = chargeMachineEntity.City,
                Latitude = chargeMachineEntity.Latitude,
                Longitude = chargeMachineEntity.Longitude,
                Number = chargeMachineEntity.Number,
                Street = chargeMachineEntity.Street
            });
        }

        [HttpGet]
        public IActionResult GetStation(int chargeMachineId) {
            var chargeMachineEntity = _applicationDbContext.ChargeMachines
                .FirstOrDefault(cm => cm.Id == chargeMachineId && cm.IsDeleted == false);

            if (chargeMachineEntity == null) {
                return Json(new {
                    error = "This station it is not valid."
                });
            }

            return Ok(new ChargeMachineViewModel() {
                Id = chargeMachineEntity.Id,
                Code = chargeMachineEntity.Code,
                City = chargeMachineEntity.City,
                Latitude = chargeMachineEntity.Latitude,
                Longitude = chargeMachineEntity.Longitude,
                Number = chargeMachineEntity.Number,
                Street = chargeMachineEntity.Street
            });
        }
    }

}