using Jake.RentalCars.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jake.RentalCars.DAL
{
    public interface IRentalRepository
    {
        Task Add(Rental rental);
        void Update(Rental rental);
        Task<Rental> Get(string bookingNumber);
        Task<Rental> Get(long rentalId);
        Task<List<Rental>> GetRentals(long rentalCarId);
    }

    public sealed class RentalRepository : IRentalRepository
    {
        private readonly RentalCarsContext context;

        public RentalRepository(RentalCarsContext context)
        {
            this.context = context;
        }

        public async Task Add(Rental rental)
        {
            await this.context.Rental.AddAsync(rental);
        }

        public Task<Rental> Get(string bookingNumber)
        {
            return this.context.Rental
                .Include(x => x.Customer)
                .Include(x => x.RentalCar)
                .SingleOrDefaultAsync(x => x.BookingNumber == bookingNumber);
        }

        public Task<Rental> Get(long rentalId)
        {
            return this.context.Rental
                .Include(x => x.Customer)
                .Include(x => x.RentalCar.CarCategory)
                .SingleOrDefaultAsync(x => x.IdRental == rentalId);
        }

        public Task<List<Rental>> GetRentals(long rentalCarId)
        {
            return this.context.Rental.Where(x => x.RentalCarId == rentalCarId).ToListAsync();
        }

        public void Update(Rental rental)
        {
            this.context.Rental.Update(rental);
        }
    }
}
