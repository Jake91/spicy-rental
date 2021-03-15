using Jake.RentalCars.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Jake.RentalCars.DAL
{
    public interface IRentalCarRepository
    {
        Task Add(RentalCar rentalCar);
        Task<RentalCar> Get(long rentalCarId);
        void Update(RentalCar rentalCar);
    }

    public sealed class RentalCarRepository : IRentalCarRepository
    {
        private readonly RentalCarsContext context;

        public RentalCarRepository(RentalCarsContext context)
        {
            this.context = context;
        }

        public Task<RentalCar> Get(long rentalCarId)
        {
            return this.context.RentalCar
                .Include(x => x.CarCategory)
                .SingleOrDefaultAsync(x => x.IdRentalCar == rentalCarId);
        }

        public async Task Add(RentalCar rentalCar)
        {
            await this.context.RentalCar.AddAsync(rentalCar);
        }

        public void Update(RentalCar rentalCar)
        {
            this.context.RentalCar.Update(rentalCar);
        }
    }
}
