using Jake.RentalCars.BLL;
using Jake.RentalCars.DAL;
using Jake.RentalCars.DAL.Models;
using System;
using System.Threading.Tasks;

namespace RentalCars.Console
{
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            await Run();
        }

        public static async Task Run()
        {
            var context = new RentalCarsContext();
            var rentalService = new RentalService(
                unitOfWork: new UnitOfWork(context, new RentalRepository(context), new RentalCarRepository(context), new CustomerRepository(context), new CarCategoryRepository(context)),
                priceCalculator: new PriceCalculator(new Settings(100, 200)));

            //var rental = await rentalService.RegisterRental(from: DateTime.Now, to: DateTime.Now.AddDays(1), carMilageKm: 144, customerId: 1, rentalCarId: 1);
            var newCustomer = new NewCustomer("Test", $"{Guid.NewGuid()}@test.se", new DateTime(year: 2021, month: 03, day: 15));
            var rental = await rentalService.RegisterRentalAndCreateCustomer(from: DateTime.Now, to: DateTime.Now.AddDays(1), carMilageKm: 144, newCustomer: newCustomer, rentalCarId: 1);

            rental = await rentalService.RentalReturn(bookingNumber: rental.BookingNumber, returnedAt: DateTime.Now.AddMinutes(1), carMilageKm: 150);
        }
    }
}
