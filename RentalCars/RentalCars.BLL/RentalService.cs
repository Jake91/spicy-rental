using Jake.RentalCars.BLL.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jake.RentalCars.BLL
{
    public interface IRentalService
    {
        Task<Rental> RegisterRentalAndCreateCustomer(DateTime from, DateTime to, int carMilageKm, NewCustomer newCustomer, long rentalCarId);

        Task<Rental> RegisterRental(DateTime from, DateTime to, int carMilageKm, long customerId, long rentalCarId);

        Task<Rental> RentalReturn(string bookingNumber, DateTime returnedAt, int carMilageKm);
    }

    public sealed class RentalService : IRentalService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPriceCalculator priceCalculator;
        private readonly RentalConverter rentalConverter;

        public RentalService(IUnitOfWork unitOfWork, IPriceCalculator priceCalculator)
        {
            this.unitOfWork = unitOfWork;
            this.priceCalculator = priceCalculator;
            this.rentalConverter = new RentalConverter();
        }

        public async Task<Rental> RegisterRentalAndCreateCustomer(DateTime from, DateTime to, int carMilageKm, NewCustomer newCustomer, long rentalCarId)
        {
            var customer = new DAL.Models.Customer()
            {
                Name = newCustomer.Name,
                Email = newCustomer.Email,
                DateOfBirth = newCustomer.DateOfBirth,
            };
            await this.unitOfWork.Customer.Add(customer);
            await this.unitOfWork.CompleteAsync();
            return await this.RegisterRental(from, to, carMilageKm, customer.IdCustomer, rentalCarId);
        }

        public async Task<Rental> RegisterRental(DateTime from, DateTime to, int carMilageKm, long customerId, long rentalCarId)
        {   
            var carRentals = await this.unitOfWork.Rental.GetRentals(rentalCarId: rentalCarId);
            if (carRentals.Any(c => c.ReturnedAt == null && IsOverlapping(interval1: (c.From, c.To), interval2: (from, to))))
            {
                throw new ArgumentException("Car is not available!");
            }
            var rentalCar = await this.unitOfWork.RentalCar.Get(rentalCarId: rentalCarId);
            rentalCar.MilageKm = carMilageKm;
            this.unitOfWork.RentalCar.Update(rentalCar: rentalCar);

            var bookingNumber = Guid.NewGuid().ToString();
            var newRental = new DAL.Models.Rental()
            {
                BookingNumber = bookingNumber,
                From = from,
                To = to,
                RentalCarId = rentalCarId,
                CustomerId = customerId,
                ReturnedAt = null,
                PaidPrice = null,
            };
            await this.unitOfWork.Rental.Add(rental: newRental);
            await this.unitOfWork.CompleteAsync();

            return this.rentalConverter.Convert(rental: newRental);
        }

        public async Task<Rental> RentalReturn(string bookingNumber, DateTime returnedAt, int carMilageKm)
        {
            var rental = await this.unitOfWork.Rental.Get(bookingNumber: bookingNumber);
            
            var rentalCar = await this.unitOfWork.RentalCar.Get(rentalCarId: rental.RentalCarId);
            rentalCar.MilageKm = carMilageKm;
            this.unitOfWork.RentalCar.Update(rentalCar: rentalCar);

            var paidPrice = this.priceCalculator.CalculatePrice(
                from: rental.From,
                to: rental.To,
                this.rentalConverter.Convert(rental.RentalCar.CarCategory),
                milageKmFrom: rental.RentalCar.MilageKm,
                milageKmTo: carMilageKm);
            rental.PaidPrice = paidPrice;
            rental.ReturnedAt = returnedAt;
            this.unitOfWork.Rental.Update(rental: rental);

            await this.unitOfWork.CompleteAsync();

            return this.rentalConverter.Convert(rental: rental);
        }

        private bool IsOverlapping((DateTime from, DateTime to) interval1, (DateTime from, DateTime to) interval2)
        {
            var interval1IsFirstAndInterval2StartsBeforeInterval1Finish = interval1.from < interval2.from && interval1.to > interval2.from;
            var interval2IsFirstAndInterval1StartsBeforeInterval2Finish = interval2.from < interval1.from && interval2.to > interval1.from;
            return interval1.from == interval2.from ||
                interval1IsFirstAndInterval2StartsBeforeInterval1Finish ||
                interval2IsFirstAndInterval1StartsBeforeInterval2Finish;
        }
    }
}
