INSERT INTO `CarCategory`
(`Name`,`DayPriceMultiplier`,`KilometerPriceMultiplier`) VALUES
("Compact", 1, 0);

INSERT INTO `CarCategory`
(`Name`,`DayPriceMultiplier`,`KilometerPriceMultiplier`) VALUES
("Premium", 1.2, 1);

INSERT INTO `CarCategory`
(`Name`,`DayPriceMultiplier`,`KilometerPriceMultiplier`) VALUES
("Minivan", 1.7, 1.5);


INSERT INTO `Customer`
(`Name`,`Email`,`DateOfBirth`) VALUES
("Jake", "jake@test.se", "2020-03-16 12:00:00");


INSERT INTO `RentalCar`
(`CarCategoryId`,`MilageKm`) VALUES
((SELECT Id_CarCategory FROM `CarCategory` WHERE Name = "Compact"), 0);

INSERT INTO `RentalCar`
(`CarCategoryId`,`MilageKm`) VALUES
((SELECT Id_CarCategory FROM `CarCategory` WHERE Name = "Premium"), 0);

INSERT INTO `RentalCar`
(`CarCategoryId`,`MilageKm`) VALUES
((SELECT Id_CarCategory FROM `CarCategory` WHERE Name = "Minivan"), 0);