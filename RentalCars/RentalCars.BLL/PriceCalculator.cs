using Jake.RentalCars.BLL.Models;
using System;

namespace Jake.RentalCars.BLL
{
    public interface IPriceCalculator
    {
        double CalculatePrice(DateTime from, DateTime to, CarCategory category, int milageKmFrom, int milageKmTo);
    }

    public sealed class PriceCalculator : IPriceCalculator
    {
        private readonly ISettings settings;

        public PriceCalculator(ISettings settings)
        {
            this.settings = settings;
        }

        public double CalculatePrice(DateTime from, DateTime to, CarCategory category, int milageKmFrom, int milageKmTo)
        {
            var numberOfDays = (int)Math.Ceiling(to.Subtract(from).TotalDays);
            var numberOfKilometers = milageKmTo - milageKmFrom;
            if (numberOfDays < 0)
            {
                throw new ArgumentException($"Incorrect parameters '{nameof(from)}' and '{nameof(to)}', {nameof(numberOfDays)}: '{numberOfDays}' is less than 0. Can not calculate price.");
            }
            if (numberOfKilometers < 0)
            {
                throw new ArgumentException($"Incorrect parameters '{nameof(milageKmFrom)}' and '{nameof(milageKmTo)}', {nameof(numberOfKilometers)}: '{numberOfKilometers}' is less than 0. Can not calculate price.");
            }
            var rentalPriceForDays = this.settings.BaseDayRental * numberOfDays * category.DayPriceMultiplier;
            var rentalPriceForUsedKm = this.settings.KilometerPrice * numberOfKilometers * category.KilometerPriceMultiplier;
            return rentalPriceForDays + rentalPriceForUsedKm;
        }
    }
}
