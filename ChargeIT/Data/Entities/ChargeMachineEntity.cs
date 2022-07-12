using System.ComponentModel.DataAnnotations;

namespace ChargeIT.Data.Entities {

    public class ChargeMachineEntity : Entity {
        public Guid Code { get; set; }
        public int Number { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        [MaxLength(64)]
        public string City { get; set; }

        [MaxLength(128)]
        public string Street { get; set; }
    }

}