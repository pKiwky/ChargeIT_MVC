using System.ComponentModel.DataAnnotations;

namespace ChargeIT.Data.Entities {

    public class CarEntity : Entity {
        [MaxLength(16)]
        public string PlateNumber { get; set; }

        public int OwnerId { get; set; }
        public CarOwnerEntity Owner { get; set; }
    }

}