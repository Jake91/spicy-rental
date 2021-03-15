using System;

namespace Jake.RentalCars.BLL.Models
{
    public sealed class Customer
    {
        public Customer(
            long idCustomer,
            string name,
            string email,
            DateTime dateOfBirth)
        {
            IdCustomer = idCustomer;
            Name = name;
            Email = email;
            DateOfBirth = dateOfBirth;
        }

        public long IdCustomer { get; }
        public string Name { get; }
        public string Email { get; }
        public DateTime DateOfBirth { get; }
    }
}
