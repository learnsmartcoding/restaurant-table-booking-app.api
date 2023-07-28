--Ctrl + N => Open new connection and new query window
-- Ctrl + X => executes the entire query, if query selcted and ran, only that selcted query runs
-- Check if the database exists
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'RestaurantTableBooking')
BEGIN
    -- Create the database
    CREATE DATABASE RestaurantTableBooking;
END

Go
use RestaurantTableBooking
go

-- Create Restaurants Table
CREATE TABLE Restaurants (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Address VARCHAR(200) NOT NULL,
    Phone VARCHAR(20),
    Email VARCHAR(100),
    ImageURL VARCHAR(500)
);

-- Create RestaurantBranches Table
CREATE TABLE RestaurantBranches (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RestaurantId INT NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Address VARCHAR(200) NOT NULL,
	Phone VARCHAR(20),
    Email VARCHAR(100),
	ImageURL VARCHAR(500),
    CONSTRAINT FK_RestaurantBranches_Restaurants
    FOREIGN KEY (RestaurantId) REFERENCES Restaurants(Id)
);

-- Create DiningTables Table
CREATE TABLE DiningTables (
    Id INT PRIMARY KEY IDENTITY(1,1),
    BranchId INT NOT NULL,
	SeatsName VARCHAR(100) NULL,
    Capacity INT NOT NULL, --default 2, 
    CONSTRAINT FK_DiningTables_RestaurantBranches
    FOREIGN KEY (BranchId) REFERENCES RestaurantBranches(Id)
);

-- Create TimeSlots Table
CREATE TABLE TimeSlots (
    Id INT PRIMARY KEY IDENTITY(1,1),
    BranchId INT NOT NULL,
	ReservationDay  DATE NOT NULL, -- day in which food served/restraurant is open.
	MealType VARCHAR(100) NOT NULL, --BreakFast, Lunch, Dinner
	TableStatus VARCHAR(50) NOT NULL, --Occupied, Booked, Available
    CONSTRAINT FK_TimeSlots_RestaurantBranches
    FOREIGN KEY (BranchId) REFERENCES RestaurantBranches(Id)
);

-- Create Users Table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),    
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
	AdObjId VARCHAR(128) NULL,
	ProfileImageUrl VARCHAR(512) NULL
);

-- Create Reservations Table
CREATE TABLE Reservations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    TableId INT NOT NULL,
    TimeSlotId INT NOT NULL,
    ReservationDate DATE NOT NULL,
    ReservationStatus VARCHAR(20) NOT NULL,
    CONSTRAINT FK_Reservations_Users
        FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_Reservations_DiningTables
        FOREIGN KEY (TableId) REFERENCES DiningTables(Id),
    CONSTRAINT FK_Reservations_TimeSlots
        FOREIGN KEY (TimeSlotId) REFERENCES TimeSlots(Id)
);



-- Insert data into Restaurants table
INSERT INTO Restaurants (Name, Address, Phone, Email, ImageURL)
VALUES ('Awesome Restaurant', '123 Main Street', '123-456-7890', 'info@awesomerestaurant.com', 'https://www.awesomerestaurant.com/logo.jpg');

-- Insert data into RestaurantBranches table (5 branches)
INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageURL)
VALUES
    (1, 'Branch A', '456 Elm Avenue', '111-222-3333', 'branchA@awesomerestaurant.com', 'https://www.awesomerestaurant.com/branchA.jpg'),
    (1, 'Branch B', '789 Oak Drive', '444-555-6666', 'branchB@awesomerestaurant.com', 'https://www.awesomerestaurant.com/branchB.jpg'),
    (1, 'Branch C', '101 Maple Street', '777-888-9999', 'branchC@awesomerestaurant.com', 'https://www.awesomerestaurant.com/branchC.jpg'),
    (1, 'Branch D', '202 Pine Road', '123-456-7890', 'branchD@awesomerestaurant.com', 'https://www.awesomerestaurant.com/branchD.jpg'),
    (1, 'Branch E', '303 Birch Lane', '444-555-6666', 'branchE@awesomerestaurant.com', 'https://www.awesomerestaurant.com/branchE.jpg');

-- Insert data into Users table (5 users)
INSERT INTO Users (FirstName, LastName, Email, AdObjId)
VALUES
    ('John', 'Doe', 'john.doe@example.com', 'AD12345'),
    ('Jane', 'Smith', 'jane.smith@example.com', 'AD67890'),
    ('Mike', 'Johnson', 'mike.johnson@example.com', 'AD45678'),
    ('Emily', 'Brown', 'emily.brown@example.com', 'AD98765'),
    ('Chris', 'Lee', 'chris.lee@example.com', 'AD54321');

	-- Insert more data into Users table (5 more users)
INSERT INTO Users (FirstName, LastName, Email, AdObjId)
VALUES
    ('Sarah', 'Williams', 'sarah.williams@example.com', 'AD98765'),
    ('Michael', 'Smith', 'michael.smith@example.com', 'AD56789'),
    ('Jessica', 'Johnson', 'jessica.johnson@example.com', 'AD65432'),
    ('Andrew', 'Brown', 'andrew.brown@example.com', 'AD23456'),
    ('Olivia', 'Lee', 'olivia.lee@example.com', 'AD87654');


-- Insert data into DiningTables table (4 tables per branch with capacity 2)
--INSERT INTO DiningTables (BranchId, SeatsName, Capacity)
--VALUES
--    -- Branch A tables
--    (1, 'Table A1', 2),
--    (1, 'Table A2', 2),
--    (1, 'Table A3', 2),
--    (1, 'Table A4', 2),
    
--    -- Branch B tables
--    (2, 'Table B1', 2),
--    (2, 'Table B2', 2),
--    (2, 'Table B3', 2),
--    (2, 'Table B4', 2),
    
--    -- Branch C tables
--    (3, 'Table C1', 2),
--    (3, 'Table C2', 2),
--    (3, 'Table C3', 2),
--    (3, 'Table C4', 2),
    
--    -- Branch D tables
--    (4, 'Table D1', 2),
--    (4, 'Table D2', 2),
--    (4, 'Table D3', 2),
--    (4, 'Table D4', 2),
    
--    -- Branch E tables
--    (5, 'Table E1', 2),
--    (5, 'Table E2', 2),
--    (5, 'Table E3', 2),
--    (5, 'Table E4', 2);

-- Insert data into DiningTables table (4 tables per branch with capacity 2)
INSERT INTO DiningTables (BranchId, SeatsName, Capacity)
VALUES
    -- Branch A tables (Avengers theme)
    (1, 'Iron Man', 2),
    (1, 'Thor', 2),
    (1, 'Captain America', 2),
    (1, 'Hulk', 2),
    
    -- Branch B tables (Disney Characters theme)
    (2, 'Mickey Mouse', 2),
    (2, 'Minnie Mouse', 2),
    (2, 'Donald Duck', 2),
    (2, 'Goofy', 2),
    
    -- Branch C tables (Avengers theme)
    (3, 'Black Widow', 2),
    (3, 'Hawkeye', 2),
    (3, 'Black Panther', 2),
    (3, 'Doctor Strange', 2),
    
    -- Branch D tables (Disney Characters theme)
    (4, 'Cinderella', 2),
    (4, 'Snow White', 2),
    (4, 'Ariel', 2),
    (4, 'Simba', 2),
    
    -- Branch E tables (Avengers theme)
    (5, 'Scarlet Witch', 2),
    (5, 'Spider-Man', 2),
    (5, 'Ant-Man', 2),
    (5, 'Captain Marvel', 2);


-- Insert data into TimeSlots table (12 time slots per branch)
INSERT INTO TimeSlots (BranchId, ReservationDay, MealType, TableStatus)
VALUES
    -- Branch A time slots
    (1, '2023-07-30', 'Breakfast', 'Available'),
    (1, '2023-07-30', 'Breakfast', 'Available'),
    (1, '2023-07-30', 'Breakfast', 'Available'),
    (1, '2023-07-30', 'Breakfast', 'Available'),
    (1, '2023-07-30', 'Lunch', 'Available'),
    (1, '2023-07-30', 'Lunch', 'Available'),
    (1, '2023-07-30', 'Lunch', 'Available'),
    (1, '2023-07-30', 'Lunch', 'Available'),
    (1, '2023-07-30', 'Dinner', 'Available'),
    (1, '2023-07-30', 'Dinner', 'Available'),
    (1, '2023-07-30', 'Dinner', 'Available'),
    (1, '2023-07-30', 'Dinner', 'Available'),
    
    -- Branch B time slots
    (2, '2023-07-30', 'Breakfast', 'Available'),
    (2, '2023-07-30', 'Breakfast', 'Available'),
    (2, '2023-07-30', 'Breakfast', 'Available'),
    (2, '2023-07-30', 'Breakfast', 'Available'),
    (2, '2023-07-30', 'Lunch', 'Available'),
    (2, '2023-07-30', 'Lunch', 'Available'),
    (2, '2023-07-30', 'Lunch', 'Available'),
    (2, '2023-07-30', 'Lunch', 'Available'),
    (2, '2023-07-30', 'Dinner', 'Available'),
    (2, '2023-07-30', 'Dinner', 'Available'),
    (2, '2023-07-30', 'Dinner', 'Available'),
    (2, '2023-07-30', 'Dinner', 'Available'),
    
    -- Branch C time slots
    (3, '2023-07-30', 'Breakfast', 'Available'),
    (3, '2023-07-30', 'Breakfast', 'Available'),
    (3, '2023-07-30', 'Breakfast', 'Available'),
    (3, '2023-07-30', 'Breakfast', 'Available'),
    (3, '2023-07-30', 'Lunch', 'Available'),
    (3, '2023-07-30', 'Lunch', 'Available'),
    (3, '2023-07-30', 'Lunch', 'Available'),
    (3, '2023-07-30', 'Lunch', 'Available'),
    (3, '2023-07-30', 'Dinner', 'Available'),
    (3, '2023-07-30', 'Dinner', 'Available'),
    (3, '2023-07-30', 'Dinner', 'Available'),
    (3, '2023-07-30', 'Dinner', 'Available'),
    
    -- Branch D time slots
    (4, '2023-07-30', 'Breakfast', 'Available'),
    (4, '2023-07-30', 'Breakfast', 'Available'),
    (4, '2023-07-30', 'Breakfast', 'Available'),
    (4, '2023-07-30', 'Breakfast', 'Available'),
    (4, '2023-07-30', 'Lunch', 'Available'),
    (4, '2023-07-30', 'Lunch', 'Available'),
    (4, '2023-07-30', 'Lunch', 'Available'),
    (4, '2023-07-30', 'Lunch', 'Available'),
    (4, '2023-07-30', 'Dinner', 'Available'),
    (4, '2023-07-30', 'Dinner', 'Available'),
    (4, '2023-07-30', 'Dinner', 'Available'),
    (4, '2023-07-30', 'Dinner', 'Available'),
    
    -- Branch E time slots
    (5, '2023-07-30', 'Breakfast', 'Available'),
    (5, '2023-07-30', 'Breakfast', 'Available'),
    (5, '2023-07-30', 'Breakfast', 'Available'),
    (5, '2023-07-30', 'Breakfast', 'Available'),
    (5, '2023-07-30', 'Lunch', 'Available'),
    (5, '2023-07-30', 'Lunch', 'Available'),
    (5, '2023-07-30', 'Lunch', 'Available'),
    (5, '2023-07-30', 'Lunch', 'Available'),
    (5, '2023-07-30', 'Dinner', 'Available'),
    (5, '2023-07-30', 'Dinner', 'Available'),
    (5, '2023-07-30', 'Dinner', 'Available'),
    (5, '2023-07-30', 'Dinner', 'Available');


	---------------------------------------
GO
	-- Insert data into Reservations table and update TimeSlots TableStatus
BEGIN TRANSACTION;

-- User 1 books specific slots
INSERT INTO Reservations (UserId, TableId, TimeSlotId, ReservationDate, ReservationStatus)
VALUES
    (1, 1, 1, '2023-07-30', 'Booked');

UPDATE TimeSlots
SET TableStatus = 'Booked'
WHERE Id = 1;

INSERT INTO Reservations (UserId, TableId, TimeSlotId, ReservationDate, ReservationStatus)
VALUES
    (1, 5, 5, '2023-07-30', 'Booked');

UPDATE TimeSlots
SET TableStatus = 'Booked'
WHERE Id = 5;

INSERT INTO Reservations (UserId, TableId, TimeSlotId, ReservationDate, ReservationStatus)
VALUES
    (1, 9, 9, '2023-07-30', 'Booked');

UPDATE TimeSlots
SET TableStatus = 'Booked'
WHERE Id = 9;

-- User 2 books specific slots
INSERT INTO Reservations (UserId, TableId, TimeSlotId, ReservationDate, ReservationStatus)
VALUES
    (2, 2, 2, '2023-07-30', 'Booked');

UPDATE TimeSlots
SET TableStatus = 'Booked'
WHERE Id = 2;

INSERT INTO Reservations (UserId, TableId, TimeSlotId, ReservationDate, ReservationStatus)
VALUES
    (2, 6, 6, '2023-07-30', 'Booked');

UPDATE TimeSlots
SET TableStatus = 'Booked'
WHERE Id = 6;

-- User 3 books specific slots
INSERT INTO Reservations (UserId, TableId, TimeSlotId, ReservationDate, ReservationStatus)
VALUES
    (3, 3, 3, '2023-07-30', 'Booked');

UPDATE TimeSlots
SET TableStatus = 'Booked'
WHERE Id = 3;

COMMIT TRANSACTION;


GO

SELECT
    R.Id AS ReservationId,
    U.FirstName + ' ' + U.LastName AS UserName,
    RB.Name AS BranchName,
    DT.SeatsName AS TableSeatsName,
    TS.ReservationDay,
    TS.MealType,
    TS.TableStatus AS TimeSlotStatus
FROM
    Reservations R
INNER JOIN
    Users U ON R.UserId = U.Id
INNER JOIN
    DiningTables DT ON R.TableId = DT.Id
INNER JOIN
    TimeSlots TS ON R.TimeSlotId = TS.Id
INNER JOIN
    RestaurantBranches RB ON DT.BranchId = RB.Id;

	select * from TimeSlots


