using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jake.RentalCars.DAL.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Rental = new HashSet<Rental>();
        }

        public long IdCustomer { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Rental> Rental { get; set; }
    }
}
