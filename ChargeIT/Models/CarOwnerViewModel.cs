﻿using ChargeIT.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ChargeIT.Models {

    public class CarOwnerViewModel {
        public int Id { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Email { get; set; }
    }

}