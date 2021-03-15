using Jake.RentalCars.DAL;
using Jake.RentalCars.DAL.Models;
using System;
using System.Threading.Tasks;

namespace Jake.RentalCars.BLL
{
    public interface IUnitOfWork : IDisposable
    {
        IRentalRepository Rental { get; }
        IRentalCarRepository RentalCar { get; }
        ICustomerRepository Customer { get; }
        ICarCategoryRepository CarCategory { get; }
        Task<int> CompleteAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public IRentalRepository Rental { get; }
        public IRentalCarRepository RentalCar { get; }
        public ICustomerRepository Customer { get; }
        public ICarCategoryRepository CarCategory { get; }

        private readonly RentalCarsContext context;

        public UnitOfWork(
            RentalCarsContext rentalCarsContext,
            IRentalRepository rentalRepository,
            IRentalCarRepository rentalCarRepository,
            ICustomerRepository customerRepository,
            ICarCategoryRepository carCategoryRepository)
        {
            this.context = rentalCarsContext;
            Rental = rentalRepository;
            RentalCar = rentalCarRepository;
            Customer = customerRepository;
            CarCategory = carCategoryRepository;
        }

        public Task<int> CompleteAsync()
        {
            return this.context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
