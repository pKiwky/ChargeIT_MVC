using System.ComponentModel.DataAnnotations;

namespace ChargeIT.Models {

    public class AddCarViewModel {
        public int Id { get; set; }

        [MaxLength(10)]
        public string PlateNumber { get; set; }

        public int OwnerId { get; set; }
        public List<DropdownViewModel>? Owners { get; set; }
    }

}