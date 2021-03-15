using Jake.RentalCars.BLL;
using Jake.RentalCars.BLL.Models;
using NUnit.Framework;
using System;

namespace Jake.RentalCars.Tests
{
    public class PriceCalculatorTests
    {
        private PriceCalculator priceCalculator;
        private readonly CarCategory carCategoryCompact;
        private readonly CarCategory carCategoryPremium;
        private readonly CarCategory carCategoryMinivan;
        private readonly Settings settings;

        public PriceCalculatorTests()
        {
            this.carCategoryCompact = new CarCategory(idCarCategory: 1, name: "Compact", dayPriceMultiplier: 1, kilometerPriceMultiplier: 0);
            this.carCategoryPremium = new CarCategory(idCarCategory: 2, name: "Premium", dayPriceMultiplier: 1.2, kilometerPriceMultiplier: 1);
            this.carCategoryMinivan = new CarCategory(idCarCategory: 3, name: "Minivan", dayPriceMultiplier: 1.7, kilometerPriceMultiplier: 1.5);
            this.settings = new Settings(baseDayRental: 700, kilometerPrice: 3);
        }

        [SetUp]
        public void Setup()
        {
            this.priceCalculator = new PriceCalculator(this.settings);
        }

        [Test]
        public void CalculatePriceTest_CompactDoNotPayForKm()
        {
            var mars16 = new DateTime(2021, 03, 16, 10, 0, 0);
            var mars17 = new DateTime(2021, 03, 17, 10, 0, 0);
            var price = this.priceCalculator.CalculatePrice(from: mars16, to: mars17, category: this.carCategoryCompact, milageKmFrom: 100, milageKmTo: 110);

            Assert.AreEqual(expected: 700, actual: price);
        }

        [Test]
        public void CalculatePriceTest_PremiumAndMinivanDoPayForKm()
        {
            var mars16 = new DateTime(2021, 03, 16, 10, 0, 0);
            var mars17 = new DateTime(2021, 03, 17, 10, 0, 0);
            var pricePremium = this.priceCalculator.CalculatePrice(from: mars16, to: mars17, category: this.carCategoryPremium, milageKmFrom: 100, milageKmTo: 110);
            var priceMinivan = this.priceCalculator.CalculatePrice(from: mars16, to: mars17, category: this.carCategoryMinivan, milageKmFrom: 100, milageKmTo: 110);

            Assert.Multiple(() => {
                Assert.AreEqual(expected: 870, actual: pricePremium, "Incorrect premium price");
                Assert.AreEqual(expected: 1235, actual: priceMinivan, "Incorrect minivan price");
            });
        }

        [Test]
        public void CalculatePriceTest_NumberOfDaysIsRoundedUpToWholeDays()
        {
            var mars16at10 = new DateTime(2021, 03, 16, 10, 0, 0);
            var mars17at11 = new DateTime(2021, 03, 17, 11, 0, 0);
            var price = this.priceCalculator.CalculatePrice(from: mars16at10, to: mars17at11, category: this.carCategoryCompact, milageKmFrom: 100, milageKmTo: 110);

            Assert.AreEqual(expected: this.settings.BaseDayRental * 2, actual: price);
        }

        [Test]
        public void CalculatePriceTest_IncorrectMilageThrows()
        {
            var mars16at10 = new DateTime(2021, 03, 16, 10, 0, 0);
            var mars17at11 = new DateTime(2021, 03, 17, 11, 0, 0);
            Assert.Throws<ArgumentException>(() => this.priceCalculator.CalculatePrice(from: mars16at10, to: mars17at11, category: this.carCategoryCompact, milageKmFrom: 100, milageKmTo: 90));
        }

        [Test]
        public void CalculatePriceTest_IncorrectFromToThrows()
        {
            var mars16at10 = new DateTime(2021, 03, 16, 10, 0, 0);
            var mars17at11 = new DateTime(2021, 03, 17, 11, 0, 0);
            Assert.Throws<ArgumentException>(() => this.priceCalculator.CalculatePrice(from: mars17at11, to: mars16at10, category: this.carCategoryCompact, milageKmFrom: 100, milageKmTo: 110));
        }
    }
}