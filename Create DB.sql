Create Table Auto (
  PK_Auto int NOT NULL Identity(0,1) primary key,
  Marke varchar(45) NULL,
  Modell varchar(45) NULL,
  Farbe varchar(45) NULL,
  Leistung int NULL,
  Preis int NULL,
  Jahrgang date NULL,
  Treibstoff varchar(45) NULL,
  Pic varbinary(MAX) NULL
);

Create Table Feedback (
  PK_Feedback int NOT NULL identity(0,1) primary key,
  Name varchar(45) NULL,
  Email varchar(45) NULL,
  Mitteilung varchar(45) NULL,
  Service int NULL,
  Modellauswahl int NULL,
  Fahrzeugart varchar(45) NULL
);

Create Table Account (
  PK_Account int NOT NULL identity(0,1) primary key,
  Password varchar(200) NOT NULL,
  Email varchar(45) NOT NULL
);