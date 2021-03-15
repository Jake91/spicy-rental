using Jake.RentalCars.BLL;
using Jake.RentalCars.DAL;
using Jake.RentalCars.DAL.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Jake.RentalCars.Tests
{
    public class RentalServiceTests : IDisposable
    {
        private CarCategory carCategoryCompact;

        private IUnitOfWork unitOfWork;
        private IDisposable context;

        private RentalService rentalService;
        private DbContextOptions<RentalCarsContext> options;


        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            this.options = new DbContextOptionsBuilder<RentalCarsContext>()
                .UseInMemoryDatabase("Test")
                .Options;

            using(var unitOfWork = CreateUnitOfWork(new RentalCarsContext(options)))
            {
                await CreateDefaultData(unitOfWork);
            }
        }

        [SetUp]
        public void Setup()
        {
            var context = new RentalCarsContext(options);
            this.context = context;

            this.unitOfWork = CreateUnitOfWork(context);
            this.rentalService = new RentalService(unitOfWork, new PriceCalculator(new Settings(baseDayRental: 700, kilometerPrice: 3)));
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
            this.context = null;
        }

        [Test]
        public async Task RegisterRentalTest_StoreNecessaryData()
        {
            var mars16 = new DateTime(year: 2021, month: 03, day: 16, hour: 10, minute: 0, second: 0);
            var mars17 = new DateTime(year: 2021, month: 03, day: 17, hour: 10, minute: 0, second: 0);
            var milageKm = 12;
            var dateOfBirth = new DateTime(year: 2000, month: 1, day: 1);

            var rentalCar = await this.CreateAndSaveCar(
                milageKm: milageKm - 2 /* Remove a few km. (Might have fueled up car since last rent) */,
                carCategory: this.carCategoryCompact);

            var result = await this.rentalService.RegisterRentalAndCreateCustomer(
                from: mars16,
                to: mars17,
                carMilageKm: milageKm,
                newCustomer: new NewCustomer(name: "Test", email: "test@test.se", dateOfBirth: dateOfBirth),
                rentalCarId: rentalCar.IdRentalCar);

            var rentalDAL = await this.unitOfWork.Rental.Get(result.IdRental);

            Assert.Multiple(() =>
            {
                Assert.IsNotEmpty(rentalDAL.BookingNumber);
                Assert.AreEqual(expected: this.carCategoryCompact.IdCarCategory, actual: rentalDAL.RentalCar.CarCategory.IdCarCategory);
                Assert.AreEqual(expected: dateOfBirth, actual: rentalDAL.Customer.DateOfBirth);
                Assert.AreEqual(expected: mars16, actual: rentalDAL.From);
                Assert.AreEqual(expected: mars17, actual: rentalDAL.To);
                Assert.AreEqual(expected: milageKm, actual: rentalDAL.RentalCar.MilageKm);
            });
        }

        [Test]
        public async Task RegisterRentalTest_NotPossibleToRentSameCarAtTheSameTime()
        {
            var mars15 = new DateTime(year: 2021, month: 03, day: 15, hour: 10, minute: 0, second: 0);
            var mars16at10 = new DateTime(year: 2021, month: 03, day: 16, hour: 10, minute: 0, second: 0);
            var mars16at11 = new DateTime(year: 2021, month: 03, day: 16, hour: 11, minute: 0, second: 0);
            var mars17 = new DateTime(year: 2021, month: 03, day: 17, hour: 10, minute: 0, second: 0);
            var milageKm = 12;
            var dateOfBirth = new DateTime(year: 2000, month: 1, day: 1);

            var rentalCar = await this.CreateAndSaveCar(
                milageKm: milageKm - 2 /* Remove a few km. (Might have fueled up car since last rent) */,
                carCategory: this.carCategoryCompact);

            await this.rentalService.RegisterRentalAndCreateCustomer(
                from: mars16at10,
                to: mars17,
                carMilageKm: milageKm,
                newCustomer: new NewCustomer(name: "Test", email: "test3@test.se", dateOfBirth: dateOfBirth),
                rentalCarId: rentalCar.IdRentalCar);

            Assert.Multiple(() =>
            {
                Assert.ThrowsAsync<ArgumentException>(async () => await this.rentalService.RegisterRentalAndCreateCustomer(
                    from: mars16at10,
                    to: mars17,
                    carMilageKm: milageKm,
                    newCustomer: new NewCustomer(name: "Test", email: "test4@test.se", dateOfBirth: dateOfBirth),
                    rentalCarId: rentalCar.IdRentalCar));

                Assert.ThrowsAsync<ArgumentException>(async () => await this.rentalService.RegisterRentalAndCreateCustomer(
                    from: mars16at11,
                    to: mars17,
                    carMilageKm: milageKm,
                    newCustomer: new NewCustomer(name: "Test", email: "test4@test.se", dateOfBirth: dateOfBirth),
                    rentalCarId: rentalCar.IdRentalCar));

                Assert.ThrowsAsync<ArgumentException>(async () => await this.rentalService.RegisterRentalAndCreateCustomer(
                    from: mars15,
                    to: mars16at11,
                    carMilageKm: milageKm,
                    newCustomer: new NewCustomer(name: "Test", email: "test4@test.se", dateOfBirth: dateOfBirth),
                    rentalCarId: rentalCar.IdRentalCar));
            });
        }

        [Test]
        public async Task RentalReturnTest_StoreNecessaryData()
        {
            var mars16 = new DateTime(year: 2021, month: 03, day: 16, hour: 10, minute: 0, second: 0);
            var mars17 = new DateTime(year: 2021, month: 03, day: 17, hour: 10, minute: 0, second: 0);
            var milageStartKm = 12;
            var milageEndKm = 20;

            var rentalCar = await this.CreateAndSaveCar(
                milageKm: milageStartKm - 2 /* Remove a few km. (Might have fueled up car since last rent) */,
                carCategory: this.carCategoryCompact);

            var rental = await this.rentalService.RegisterRentalAndCreateCustomer(
                from: mars16,
                to: mars17,
                carMilageKm: milageStartKm,
                newCustomer: new NewCustomer(name: "Test", email: "test2@test.se", dateOfBirth: new DateTime(year: 2000, month: 1, day: 1)),
                rentalCarId: rentalCar.IdRentalCar);

            var result = await this.rentalService.RentalReturn(bookingNumber: rental.BookingNumber, returnedAt: mars17, carMilageKm: milageEndKm);

            var rentalDAL = await this.unitOfWork.Rental.Get(result.IdRental);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected: 700, actual: rentalDAL.PaidPrice);

                Assert.AreEqual(expected: mars17, actual: rentalDAL.ReturnedAt);
                Assert.AreEqual(expected: milageEndKm, actual: rentalDAL.RentalCar.MilageKm);
            });
        }

        private async Task<RentalCar> CreateAndSaveCar(int milageKm, CarCategory carCategory)
        {
            var rentalCar = new RentalCar()
            {
                CarCategoryId = carCategory.IdCarCategory,
                MilageKm = milageKm
            };

            await this.unitOfWork.RentalCar.Add(rentalCar);
            await this.unitOfWork.CompleteAsync();
            return rentalCar;
        }

        private IUnitOfWork CreateUnitOfWork(RentalCarsContext context)
        {
            return new UnitOfWork(
                rentalCarsContext: context,
                rentalRepository: new RentalRepository(context),
                rentalCarRepository: new RentalCarRepository(context),
                customerRepository: new CustomerRepository(context),
                carCategoryRepository: new CarCategoryRepository(context));
        }

        private async Task CreateDefaultData(IUnitOfWork unitOfWork)
        {
            this.carCategoryCompact = new CarCategory()
            {
                IdCarCategory = 0,
                Name = "Compact",
                DayPriceMultiplier = 1,
                KilometerPriceMultiplier = 0
            };
            await unitOfWork.CarCategory.Add(this.carCategoryCompact);
            await unitOfWork.CompleteAsync();
        }

        public void Dispose()
        {
            this.context?.Dispose();
        }
    }
}