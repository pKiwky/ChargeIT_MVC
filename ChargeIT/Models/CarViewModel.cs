using System.ComponentModel.DataAnnotations;

namespace ChargeIT.Models {

    public class CarViewModel {
        public int Id { get; set; }

        [MaxLength(10)]
        public string PlateNumber { get; set; }
    }

}