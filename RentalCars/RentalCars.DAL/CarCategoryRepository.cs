using Jake.RentalCars.DAL.Models;
using System.Threading.Tasks;

namespace Jake.RentalCars.DAL
{
    public interface ICarCategoryRepository
    {
        Task Add(CarCategory carCategory);
    }

    public sealed class CarCategoryRepository : ICarCategoryRepository
    {
        private readonly RentalCarsContext context;

        public CarCategoryRepository(RentalCarsContext context)
        {
            this.context = context;
        }

        public async Task Add(CarCategory carCategory)
        {
            await this.context.CarCategory.AddAsync(carCategory);
        }
    }
}
