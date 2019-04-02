CREATE DATABASE  IF NOT EXISTS `ip6` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ip6`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: ip6
-- ------------------------------------------------------
-- Server version	5.7.18-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `calculations`
--

DROP TABLE IF EXISTS `calculations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
) ENGINE=InnoDB AUTO_INCREMENT=62 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `estimations`
--

DROP TABLE IF EXISTS `estimations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `estimations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CPUFactor` double NOT NULL,
  `DBInsertFactor` double NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sealingslabs`
--

DROP TABLE IF EXISTS `sealingslabs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sealingslabs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CalculationId` int(11) NOT NULL,
  `Iteration` int(11) NOT NULL,
  `X` int(11) NOT NULL,
  `Y` int(11) NOT NULL,
  `Radius` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Area` (`CalculationId`,`Iteration`),
  CONSTRAINT `sealingslabs_ibfk_1` FOREIGN KEY (`CalculationId`) REFERENCES `calculations` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=21362770 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `states`
--

DROP TABLE IF EXISTS `states`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `states` (
  `Id` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Password` varchar(100) NOT NULL,
  `IsLocal` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-08-04 10:19:15


/* Add indexes */
ALTER TABLE `calculations` ADD INDEX `StateId` USING BTREE(`StateId`);
ALTER TABLE `calculations` ADD INDEX `UserId` USING BTREE(`UserId`);
ALTER TABLE `sealingslabs` ADD INDEX `Area` USING BTREE(`CalculationId`, `Iteration`);
ALTER TABLE `sealingslabs` ADD INDEX `CalculationId` USING BTREE(`CalculationId`);