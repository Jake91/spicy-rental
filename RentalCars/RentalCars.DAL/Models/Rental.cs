using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jake.RentalCars.DAL.Models
{
    public partial class Rental
    {
        public long IdRental { get; set; }
        public string BookingNumber { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long RentalCarId { get; set; }
        public long CustomerId { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public double? PaidPrice { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual RentalCar RentalCar { get; set; }
    }
}
