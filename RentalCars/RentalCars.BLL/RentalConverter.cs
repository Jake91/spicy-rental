using Jake.RentalCars.BLL.Models;
using System.Linq;

namespace Jake.RentalCars.BLL
{
    public sealed class RentalConverter
    {
        public Rental Convert(DAL.Models.Rental rental)
        {
            return new Rental(
                idRental: rental.IdRental,
                bookingNumber: rental.BookingNumber,
                from: rental.From,
                to: rental.To,
                customer: Convert(rental.Customer),
                rentalCar: Convert(rental.RentalCar),
                returnedAt: rental.ReturnedAt,
                paidPrice: rental.PaidPrice);
        }

        public RentalCar Convert(DAL.Models.RentalCar rentalCar)
        {
            return new RentalCar(
                rentalCar.IdRentalCar,
                rentalCar.MilageKm,
                Convert(rentalCar.CarCategory));
        }

        public CarCategory Convert(DAL.Models.CarCategory carCategory)
        {
            return new CarCategory(
                idCarCategory: carCategory.IdCarCategory,
                name: carCategory.Name,
                dayPriceMultiplier: carCategory.DayPriceMultiplier,
                kilometerPriceMultiplier: carCategory.KilometerPriceMultiplier);
        }

        public Customer Convert(DAL.Models.Customer customer)
        {
            return new Customer(
                idCustomer: customer.IdCustomer,
                name: customer.Name,
                email: customer.Email,
                dateOfBirth: customer.DateOfBirth);
        }
    }
}
