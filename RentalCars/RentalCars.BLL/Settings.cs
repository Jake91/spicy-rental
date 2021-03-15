namespace Jake.RentalCars.BLL
{
    public interface ISettings
    {
        double BaseDayRental { get; }
        double KilometerPrice { get; }
    }

    public class Settings : ISettings
    {
        public double BaseDayRental { get; }

        public double KilometerPrice { get; }

        public Settings(double baseDayRental, double kilometerPrice)
        {
            BaseDayRental = baseDayRental;
            KilometerPrice = kilometerPrice;
        }
    }
}
