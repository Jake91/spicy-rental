using System;

namespace Jake.RentalCars.BLL
{
    public sealed class NewCustomer
    {
        public NewCustomer(string name, string email, DateTime dateOfBirth)
        {
            Name = name;
            Email = email;
            DateOfBirth = dateOfBirth;
        }
        public string Name { get; }
        public string Email { get; }
        public DateTime DateOfBirth { get; }
    }
}
