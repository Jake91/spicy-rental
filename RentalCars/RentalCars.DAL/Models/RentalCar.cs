using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jake.RentalCars.DAL.Models
{
    public partial class RentalCar
    {
        public RentalCar()
        {
            Rental = new HashSet<Rental>();
        }

        public long IdRentalCar { get; set; }
        public long CarCategoryId { get; set; }
        public int MilageKm { get; set; }

        public virtual CarCategory CarCategory { get; set; }
        public virtual ICollection<Rental> Rental { get; set; }
    }
}
