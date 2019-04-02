CREATE TABLE `users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Password` varchar(100) NOT NULL,
  `IsLocal` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

CREATE TABLE `states` (
  `Id` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `estimations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CPUFactor` double NOT NULL,
  `DBInsertFactor` double NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

CREATE TABLE `calculations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `StateId` int(11) NOT NULL,
  `Width` decimal(7,3) NOT NULL,
  `Height` decimal(7,3) NOT NULL,
  `BorderX` decimal(7,3) NOT NULL,
  `BorderY` decimal(7,3) NOT NULL,
  `DrillingPointDistanceX` decimal(7,3) NOT NULL,
  `DrillingPointDistanceY` decimal(7,3) NOT NULL,
  `SealingSlabDiameter` decimal(7,3) NOT NULL,
  `Depth` decimal(7,3) NOT NULL,
  `PixelsPerMeter` int(11) NOT NULL,
  `StandardDerivationOffset` decimal(5,2) NOT NULL,
  `StandardDerivationRadius` decimal(5,2) NOT NULL,
  `StandardDerivationDrillingPoint` decimal(5,2) NOT NULL,
  `Iterations` int(11) NOT NULL,
  `TimeStamp` datetime NOT NULL,
  `UnsetAreaResult` decimal(5,2) DEFAULT NULL,
  `StartDate` datetime DEFAULT NULL,
  `EndDate` datetime DEFAULT NULL,
  `EstimatedEndDate` datetime DEFAULT NULL,
  `WaterLevelDifference` decimal(18,2) NOT NULL,
  `SealingSlabThickness` decimal(18,2) NOT NULL,
  `PermeabilityOfSoleWithoutUnsetArea` decimal(14,10) NOT NULL,
  `PermeabilityOfSoleAtUnsetArea` decimal(14,10) NOT NULL,
  `ResidualWaterResult` decimal(18,2) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`),
  KEY `StateId` (`StateId`),
  CONSTRAINT `calculations_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`),
  CONSTRAINT `calculations_ibfk_2` FOREIGN KEY (`StateId`) REFERENCES `states` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8;

