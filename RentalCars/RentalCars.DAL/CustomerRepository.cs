using Jake.RentalCars.DAL.Models;
using System.Threading.Tasks;

namespace Jake.RentalCars.DAL
{
    public interface ICustomerRepository
    {
        Task Add(Customer customer);
        void Update(Customer customer);
    }

    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly RentalCarsContext context;

        public CustomerRepository(RentalCarsContext context)
        {
            this.context = context;
        }

        public async Task Add(Customer customer)
        {
            await this.context.Customer.AddAsync(customer);
        }

        public void Update(Customer customer)
        {
            this.context.Customer.Update(customer);
        }
    }
}
