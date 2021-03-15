namespace Jake.RentalCars.BLL.Models
{
    // Requirement "It should be possible to add more categories later" is a bit unclear.
    // Might just be enought with an enum that we can update in code later? But since it was a requirement I guess we want to do it without modifying the code?
    // To do that I assumed that the following formula is applicable for future categories: "baseDayRental * numberOfDays * DayPriceMultiplier + (kilometerPrice * numberOfKilometers * KilometerPriceMultiplier)"
    public sealed class CarCategory
    {
        public CarCategory(
            long idCarCategory,
            string name,
            double dayPriceMultiplier,
            double kilometerPriceMultiplier)
        {
            IdCarCategory = idCarCategory;
            Name = name;
            DayPriceMultiplier = dayPriceMultiplier;
            KilometerPriceMultiplier = kilometerPriceMultiplier;
        }

        public long IdCarCategory { get; }
        public string Name { get; }
        public double DayPriceMultiplier { get; }
        public double KilometerPriceMultiplier { get; }
    }
}
