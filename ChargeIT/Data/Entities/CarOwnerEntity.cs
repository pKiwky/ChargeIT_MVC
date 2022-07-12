using System.ComponentModel.DataAnnotations;

namespace ChargeIT.Data.Entities {

    public class CarOwnerEntity : Entity {
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Email { get; set; }

        public ICollection<CarEntity> Cars { get; set; }
    }

}