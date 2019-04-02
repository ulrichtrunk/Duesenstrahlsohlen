CREATE TABLE IF NOT EXISTS users (
Id INTEGER PRIMARY KEY,
Name TEXT NOT NULL,
Password TEXT NOT NULL,
IsLocal INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS states (
Id INTEGER PRIMARY KEY,
Name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS calculations (
  Id INTEGER PRIMARY KEY,
  UserId INTEGER NOT NULL,
  StateId INTEGER NOT NULL,
  Width DOUBLE NOT NULL,
  Height DOUBLE NOT NULL,
  BorderX DOUBLE NOT NULL,
  BorderY DOUBLE NOT NULL,
  DrillingPointDistanceX DOUBLE NOT NULL,
  DrillingPointDistanceY DOUBLE NOT NULL,
  SealingSlabDiameter DOUBLE NOT NULL,
  Depth DOUBLE NOT NULL,
  PixelsPerMeter int(11) NOT NULL,
  StandardDerivationOffset DOUBLE NOT NULL,
  StandardDerivationRadius DOUBLE NOT NULL,
  StandardDerivationDrillingPoint DOUBLE NOT NULL,
  Iterations int(11) NOT NULL,
  TimeStamp DATETIME NOT NULL,
  UnsetAreaResult DECIMAL(5,2) DEFAULT NULL,
  StartDate DATETIME DEFAULT NULL,
  EndDate DATETIME DEFAULT NULL,
  EstimatedEndDate DATETIME DEFAULT NULL,
  WaterLevelDifference DECIMAL(18,2) NOT NULL,
  SealingSlabThickness DECIMAL(18,2) NOT NULL,
  PermeabilityOfSoleWithoutUnsetArea DECIMAL(14,10) NOT NULL,
  PermeabilityOfSoleAtUnsetArea DECIMAL(14,10) NOT NULL,
  ResidualWaterResult DECIMAL(18,2) DEFAULT NULL,
  FOREIGN KEY (UserId) REFERENCES users (Id),
  FOREIGN KEY (StateId) REFERENCES states (Id) 
);

CREATE TABLE IF NOT EXISTS sealingslabs (
Id INTEGER PRIMARY KEY,
CalculationId INTEGER NOT NULL,
Iteration INTEGER NOT NULL,
BaseX INTEGER NOT NULL,
BaseY INTEGER NOT NULL,
OffsetX INTEGER NOT NULL,
OffsetY INTEGER NOT NULL,
OffsetDrillingPointX INTEGER NOT NULL,
OffsetDrillingPointY INTEGER NOT NULL,
X INTEGER NOT NULL,
Y INTEGER NOT NULL,
Radius INTEGER NOT NULL,
FOREIGN KEY (CalculationId) REFERENCES calculations (Id) 
);

CREATE TABLE IF NOT EXISTS estimations (
Id INTEGER PRIMARY KEY,
CPUFactor DOUBLE NOT NULL,
DBInsertFactor DOUBLE NOT NULL
);

INSERT INTO states (Id, Name) VALUES (1, 'Enqueued');
INSERT INTO states (Id, Name) VALUES (2, 'Running');
INSERT INTO states (Id, Name) VALUES (3, 'Cancelling');
INSERT INTO states (Id, Name) VALUES (4, 'Cancelled');
INSERT INTO states (Id, Name) VALUES (5, 'Error');
INSERT INTO states (Id, Name) VALUES (6, 'Done');
INSERT INTO states (Id, Name) VALUES (7, 'Deleting');