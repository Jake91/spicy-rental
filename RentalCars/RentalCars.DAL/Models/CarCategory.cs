using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jake.RentalCars.DAL.Models
{
    public partial class CarCategory
    {
        public CarCategory()
        {
            RentalCar = new HashSet<RentalCar>();
        }

        public long IdCarCategory { get; set; }
        public string Name { get; set; }
        public double DayPriceMultiplier { get; set; }
        public double KilometerPriceMultiplier { get; set; }

        public virtual ICollection<RentalCar> RentalCar { get; set; }
    }
}
