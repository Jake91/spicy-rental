using System;

namespace Jake.RentalCars.BLL.Models
{
    public sealed class Rental
    {

        public Rental(
            long idRental,
            string bookingNumber,
            DateTime from,
            DateTime to,
            Customer customer,
            RentalCar rentalCar,
            DateTime? returnedAt,
            double? paidPrice)
        {
            IdRental = idRental;
            BookingNumber = bookingNumber;
            From = from;
            To = to;
            ReturnedAt = returnedAt;
            PaidPrice = paidPrice;
            Customer = customer;
            RentalCar = rentalCar;
        }

        public long IdRental { get; }
        public string BookingNumber { get; }
        public DateTime From { get; }
        public DateTime To { get; }
        public DateTime? ReturnedAt { get; }
        public double? PaidPrice { get; }

        public Customer Customer { get; }
        public RentalCar RentalCar { get; }
    }
}
