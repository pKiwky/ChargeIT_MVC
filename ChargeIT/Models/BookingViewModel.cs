using System.ComponentModel.DataAnnotations;

namespace ChargeIT.Models {
    
    public class BookingViewModel {
        public  Guid Code { get; set; }
        
        [Display(Name = "Charge machine")]
        public int ChargeMachineId { get; set; }
        public List<DropdownViewModel>? ChargeMachines { get; set; }

        [Display(Name = "Car")]
        public int CarId { get; set; }
        public List<DropdownViewModel>? Cars { get; set; }

        public DateTime? Date { get; set; }

        [Range(0, 24)]
        public int IntervalHour { get; set; }
    }
    
}
