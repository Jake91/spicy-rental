namespace Jake.RentalCars.BLL.Models
{
    public sealed class RentalCar
    {
        public RentalCar(
            long idRentalCar,
            int milageKm,
            CarCategory carCategory)
        {
            IdRentalCar = idRentalCar;
            MilageKm = milageKm;
            CarCategory = carCategory;
        }

        public long IdRentalCar { get; }
        public int MilageKm { get; }

        public CarCategory CarCategory { get; }
    }
}
