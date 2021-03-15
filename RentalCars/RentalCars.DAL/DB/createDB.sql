CREATE TABLE `CarCategory` (
  `Id_CarCategory` INTEGER PRIMARY KEY,
  `Name` NVARCHAR(100) NOT NULL UNIQUE,
  `DayPriceMultiplier` DOUBLE NOT NULL,
  `KilometerPriceMultiplier` DOUBLE NOT NULL);

CREATE TABLE `RentalCar` (
  `Id_RentalCar` INTEGER PRIMARY KEY,
  `CarCategoryId` INTEGER NOT NULL,
  `MilageKm` INTEGER NOT NULL,
  FOREIGN KEY (CarCategoryId) REFERENCES CarCategory(Id_CarCategory));

CREATE TABLE `Customer` (
  `Id_Customer` INTEGER PRIMARY KEY,
  `Name` NVARCHAR(100) NOT NULL,
  `Email` NVARCHAR(350) NOT NULL UNIQUE,
  `DateOfBirth` DATETIME NOT NULL);

  CREATE TABLE `Rental` (
  `Id_Rental` INTEGER PRIMARY KEY,
  `BookingNumber` NVARCHAR(36) NOT NULL UNIQUE,
  `From` DATETIME NOT NULL,
  `To` DATETIME NOT NULL,
  `RentalCarId` INTEGER NOT NULL,
  `CustomerId` INTEGER NOT NULL,
  `ReturnedAt` DATETIME NULL,
  `PaidPrice` DOUBLE NULL,
  FOREIGN KEY (RentalCarId) REFERENCES RentalCar(Id_RentalCar),
  FOREIGN KEY (CustomerId) REFERENCES Customer(Id_Customer));