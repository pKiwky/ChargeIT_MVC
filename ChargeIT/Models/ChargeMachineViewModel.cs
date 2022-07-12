using System.ComponentModel.DataAnnotations;

namespace ChargeIT.Models {

    public class ChargeMachineViewModel {
        public int Id { get; set; }
        public int Number { get; set; }
        public Guid Code { get; set; }
        
        [MaxLength(64)]
        public string City { get; set; }

        [MaxLength(64)]
        public string Street { get; set; }

        [Range(-90.0, 90.0)]
        public double? Longitude { get; set; }

        [Range(-90.0, 90.0)]
        public double? Latitude { get; set; }
    }

}