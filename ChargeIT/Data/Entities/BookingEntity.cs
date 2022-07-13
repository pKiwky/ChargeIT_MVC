namespace ChargeIT.Data.Entities {

    public class BookingEntity : Entity {
        public Guid Code { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int ChargeMachineId { get; set; }
        public ChargeMachineEntity ChargeMachine { get; set; }
        
        public int CarId { get; set; }
        public CarEntity Car { get; set; }
    }

}