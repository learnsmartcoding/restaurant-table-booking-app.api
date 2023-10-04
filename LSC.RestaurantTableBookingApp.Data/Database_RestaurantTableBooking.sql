--Ctrl + N => Open new connection and new query window
-- Ctrl + X => executes the entire query, if query selcted and ran, only that selcted query runs
-- Check if the database exists
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'RestaurantTableBooking')
BEGIN
    -- Create the database
    CREATE DATABASE RestaurantTableBooking;
END
ELSE
BEGIN
   DROP DATABASE RestaurantTableBooking;
END

Go
use RestaurantTableBooking
go

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Restaurants] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Address] nvarchar(200) NOT NULL,
    [Phone] nvarchar(20) NULL,
    [Email] nvarchar(100) NULL,
    [ImageUrl] nvarchar(500) NULL,
    CONSTRAINT [PK_Restaurants] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [AdObjId] nvarchar(128) NULL,
    [ProfileImageUrl] nvarchar(512) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [UpdatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [RestaurantBranches] (
    [Id] int NOT NULL IDENTITY,
    [RestaurantId] int NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Address] nvarchar(200) NOT NULL,
    [Phone] nvarchar(20) NULL,
    [Email] nvarchar(100) NULL,
    [ImageUrl] nvarchar(500) NULL,
    CONSTRAINT [PK_RestaurantBranches] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RestaurantBranches_Restaurants_RestaurantId] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurants] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [DiningTables] (
    [Id] int NOT NULL IDENTITY,
    [RestaurantBranchId] int NOT NULL,
    [TableName] nvarchar(100) NULL,
    [Capacity] int NOT NULL,
    CONSTRAINT [PK_DiningTables] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DiningTables_RestaurantBranches_RestaurantBranchId] FOREIGN KEY ([RestaurantBranchId]) REFERENCES [RestaurantBranches] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [TimeSlots] (
    [Id] int NOT NULL IDENTITY,
    [DiningTableId] int NOT NULL,
    [ReservationDay] datetime2 NOT NULL,
    [MealType] nvarchar(max) NOT NULL,
    [TableStatus] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_TimeSlots] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TimeSlots_DiningTables_DiningTableId] FOREIGN KEY ([DiningTableId]) REFERENCES [DiningTables] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Reservations] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [TimeSlotId] int NOT NULL,
    [ReservationDate] datetime2 NOT NULL,
    [ReservationStatus] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Reservations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Reservations_TimeSlots_TimeSlotId] FOREIGN KEY ([TimeSlotId]) REFERENCES [TimeSlots] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reservations_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_DiningTables_RestaurantBranchId] ON [DiningTables] ([RestaurantBranchId]);
GO

CREATE INDEX [IX_Reservations_TimeSlotId] ON [Reservations] ([TimeSlotId]);
GO

CREATE INDEX [IX_Reservations_UserId] ON [Reservations] ([UserId]);
GO

CREATE INDEX [IX_RestaurantBranches_RestaurantId] ON [RestaurantBranches] ([RestaurantId]);
GO

CREATE INDEX [IX_TimeSlots_DiningTableId] ON [TimeSlots] ([DiningTableId]);
GO

ALTER TABLE [Reservations] ADD [ReminderSent] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230729134945_dbdesignchange', N'7.0.9');
GO

COMMIT;
GO


INSERT INTO Restaurants (Name, Address, Phone, Email, ImageUrl)
VALUES ('Awesome Restaurant', '123 Main Street', '555-123-4567', 'info@awesomerestaurant.com', 'https://www.awesomerestaurant.com/image.jpg');
INSERT INTO Restaurants (Name, Address, Phone, Email, ImageUrl)
VALUES ('Delicious Diner', '456 Oak Avenue', '555-987-6543', 'info@deliciousdiner.com', 'https://www.deliciousdiner.com/image.jpg');

INSERT INTO Restaurants (Name, Address, Phone, Email, ImageUrl)
VALUES ('Tasty Bistro', '789 Elm Street', '555-456-7890', 'info@tastybistro.com', 'https://www.tastybistro.com/image.jpg');

INSERT INTO Restaurants (Name, Address, Phone, Email, ImageUrl)
VALUES ('Gourmet Grille', '321 Pine Road', '555-789-0123', 'info@gourmetgrille.com', 'https://www.gourmetgrille.com/image.jpg');

INSERT INTO Restaurants (Name, Address, Phone, Email, ImageUrl)
VALUES ('Cafe Fusion', '567 Maple Lane', '555-234-5678', 'info@cafefusion.com', 'https://www.cafefusion.com/image.jpg');

INSERT INTO Restaurants (Name, Address, Phone, Email, ImageUrl)
VALUES ('Savory Spots', '890 Birch Court', '555-678-9012', 'info@savoryspots.com', 'https://www.savoryspots.com/image.jpg');

INSERT INTO Restaurants (Name, Address, Phone, Email, ImageUrl)
VALUES ('Elegant Eats', '123 Cherry Avenue', '555-901-2345', 'info@eleganteats.com', 'https://www.eleganteats.com/image.jpg');

INSERT INTO Restaurants (Name, Address, Phone, Email, ImageUrl)
VALUES ('Urban Kitchen', '456 Plum Street', '555-345-6789', 'info@urbankitchen.com', 'https://www.urbankitchen.com/image.jpg');

INSERT INTO Restaurants (Name, Address, Phone, Email, ImageUrl)
VALUES ('Mouthwatering Meals', '789 Berry Road', '555-678-1234', 'info@mouthwateringmeals.com', 'https://www.mouthwateringmeals.com/image.jpg');

INSERT INTO Restaurants (Name, Address, Phone, Email, ImageUrl)
VALUES ('Trendy Tastes', '321 Orange Lane', '555-234-5678', 'info@trendytastes.com', 'https://www.trendytastes.com/image.jpg');

INSERT INTO Restaurants (Name, Address, Phone, Email, ImageUrl)
VALUES ('Flavorsome Fare', '567 Lemon Court', '555-901-2345', 'info@flavorsomefare.com', 'https://www.flavorsomefare.com/image.jpg');


INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageUrl)
VALUES
    (1, 'Branch A', '456 Oak Avenue', '555-789-1234', 'branchA@awesomerestaurant.com', 'https://www.awesomerestaurant.com/branchA.jpg'),
    (1, 'Branch B', '789 Oak Drive', '555-222-3333', 'branchB@awesomerestaurant.com', 'https://www.awesomerestaurant.com/branchB.jpg'),
    (1, 'Branch C', '789 Maple Lane', '555-444-5555', 'branchC@awesomerestaurant.com', 'https://www.awesomerestaurant.com/branchC.jpg'),
    (1, 'Branch D', '123 Elm Street', '555-666-7777', 'branchD@awesomerestaurant.com', 'https://www.awesomerestaurant.com/branchD.jpg'),
    (1, 'Branch E', '987 Pine Road', '555-888-9999', 'branchE@awesomerestaurant.com', 'https://www.awesomerestaurant.com/branchE.jpg');

    INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageUrl)
VALUES
    (1, 'Downtown Branch', '789 Main Street', '555-111-2222', 'downtown@awesomerestaurant.com', 'https://www.awesomerestaurant.com/downtown.jpg'),
    (1, 'Eastside Branch', '456 Park Avenue', '555-333-4444', 'eastside@awesomerestaurant.com', 'https://www.awesomerestaurant.com/eastside.jpg');

	INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageUrl)
VALUES
    (2, 'City Center Branch', '123 Broadway Street', '555-555-5555', 'citycenter@deliciousdiner.com', 'https://www.deliciousdiner.com/citycenter.jpg'),
    (2, 'Westend Branch', '789 Market Road', '555-777-7777', 'westend@deliciousdiner.com', 'https://www.deliciousdiner.com/westend.jpg');
	INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageUrl)
VALUES
    (3, 'Main Square Branch', '321 Park Lane', '555-999-9999', 'mainsquare@tastybistro.com', 'https://www.tastybistro.com/mainsquare.jpg'),
    (3, 'Northside Branch', '987 Maple Avenue', '555-222-3333', 'northside@tastybistro.com', 'https://www.tastybistro.com/northside.jpg');
	INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageUrl)
VALUES
    (4, 'Gourmet Plaza Branch', '654 Oak Lane', '555-444-4444', 'gourmetplaza@gourmetgrille.com', 'https://www.gourmetgrille.com/gourmetplaza.jpg'),
    (4, 'Southside Branch', '321 Elm Road', '555-666-6666', 'southside@gourmetgrille.com', 'https://www.gourmetgrille.com/southside.jpg');

	INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageUrl)
VALUES
    (5, 'Fusion Central Branch', '789 Market Street', '555-888-8888', 'fusioncentral@cafefusion.com', 'https://www.cafefusion.com/fusioncentral.jpg'),
    (5, 'Seaside Branch', '456 Beach Road', '555-111-1111', 'seaside@cafefusion.com', 'https://www.cafefusion.com/seaside.jpg');

	INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageUrl)
VALUES
    (6, 'Steakhouse Heights Branch', '123 Hill Street', '555-111-0000', 'steakhouseheights@sizzlingsteakhouse.com', 'https://www.sizzlingsteakhouse.com/steakhouseheights.jpg'),
    (6, 'Westside Branch', '789 Sunset Road', '555-222-1111', 'westside@sizzlingsteakhouse.com', 'https://www.sizzlingsteakhouse.com/westside.jpg');
	INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageUrl)
VALUES
    (7, 'Paradise Park Branch', '654 Garden Avenue', '555-333-2222', 'paradisepark@pastaparadise.com', 'https://www.pastaparadise.com/paradisepark.jpg'),
    (7, 'Central Square Branch', '987 Center Street', '555-444-3333', 'centralsquare@pastaparadise.com', 'https://www.pastaparadise.com/centralsquare.jpg');



	INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageUrl)
VALUES
    (8, 'Fusion Heights Branch', '321 Tower Road', '555-555-4444', 'fusionheights@asianfusion.com', 'https://www.asianfusion.com/fusionheights.jpg'),
    (8, 'East End Branch', '789 Harbor Lane', '555-666-5555', 'eastend@asianfusion.com', 'https://www.asianfusion.com/eastend.jpg');

	INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageUrl)
VALUES
    (9, 'Delightful Square Branch', '456 Garden Lane', '555-777-6666', 'delightfulsquare@mediterraneandelight.com', 'https://www.mediterraneandelight.com/delightfulsquare.jpg'),
    (9, 'North End Branch', '123 Harbor Avenue', '555-888-7777', 'northend@mediterraneandelight.com', 'https://www.mediterraneandelight.com/northend.jpg');


	INSERT INTO RestaurantBranches (RestaurantId, Name, Address, Phone, Email, ImageUrl)
VALUES
    (10, 'Green Oasis Branch', '321 Forest Road', '555-999-8888', 'greenoasis@vegetarianoasis.com', 'https://www.vegetarianoasis.com/greenoasis.jpg'),
    (10, 'Westside Oasis Branch', '789 River Lane', '555-111-9999', 'westsideoasis@vegetarianoasis.com', 'https://www.vegetarianoasis.com/westsideoasis.jpg');


INSERT INTO Users (FirstName, LastName, Email, AdObjId, ProfileImageUrl, CreatedDate, UpdatedDate)
VALUES
    ('John', 'Doe', 'john.doe@example.com', '123456', 'https://www.example.com/john.jpg', GETDATE(), GETDATE()),
    ('Jane', 'Smith', 'jane.smith@example.com', '789012', 'https://www.example.com/jane.jpg', GETDATE(), GETDATE()),
    ('Mike', 'Johnson', 'mike.johnson@example.com', '345678', 'https://www.example.com/mike.jpg', GETDATE(), GETDATE()),
    ('Emily', 'Davis', 'emily.davis@example.com', '901234', 'https://www.example.com/emily.jpg', GETDATE(), GETDATE()),
    ('Robert', 'Brown', 'robert.brown@example.com', '567890', 'https://www.example.com/robert.jpg', GETDATE(), GETDATE());


    INSERT INTO DiningTables (RestaurantBranchId, TableName, Capacity)
VALUES
    (1, 'Mickey Mouse', 2),
    (1, 'Minnie Mouse', 2),
    (1, 'Donald Duck', 2),
    (1, 'Goofy', 2),

    (2, 'Tom Cat', 2),
    (2, 'Jerry Mouse', 2),
    (2, 'Scooby Doo', 2),
    (2, 'Shaggy Rogers', 2),

    (3, 'Batman', 2),
    (3, 'Superman', 2),
    (3, 'Wonder Woman', 2),
    (3, 'The Flash', 2),

    (4, 'Spider-Man', 2),
    (4, 'Iron Man', 2),
    (4, 'Captain America', 2),
    (4, 'Thor', 2),

    (5, 'Homer Simpson', 2),
    (5, 'Marge Simpson', 2),
    (5, 'Bart Simpson', 2),
    (5, 'Lisa Simpson', 2);

INSERT INTO TimeSlots (DiningTableId, ReservationDay, MealType, TableStatus)
VALUES
    -- Branch A time slots
    (1, '2023-07-30', 'Breakfast', 'Available'),
    (2, '2023-07-30', 'Breakfast', 'Available'),
    (3, '2023-07-30', 'Breakfast', 'Available'),
    (4, '2023-07-30', 'Breakfast', 'Available'),

    (1, '2023-07-30', 'Lunch', 'Available'),
    (2, '2023-07-30', 'Lunch', 'Available'),
    (3, '2023-07-30', 'Lunch', 'Available'),
    (4, '2023-07-30', 'Lunch', 'Available'),

    (1, '2023-07-30', 'Dinner', 'Available'),
    (2, '2023-07-30', 'Dinner', 'Available'),
    (3, '2023-07-30', 'Dinner', 'Available'),
    (4, '2023-07-30', 'Dinner', 'Available'),

    -- Branch B time slots
    (5, '2023-07-30', 'Breakfast', 'Available'),
    (6, '2023-07-30', 'Breakfast', 'Available'),
    (7, '2023-07-30', 'Breakfast', 'Available'),
    (8, '2023-07-30', 'Breakfast', 'Available'),

    (5, '2023-07-30', 'Lunch', 'Available'),
    (6, '2023-07-30', 'Lunch', 'Available'),
    (7, '2023-07-30', 'Lunch', 'Available'),
    (8, '2023-07-30', 'Lunch', 'Available'),

    (5, '2023-07-30', 'Dinner', 'Available'),
    (6, '2023-07-30', 'Dinner', 'Available'),
    (7, '2023-07-30', 'Dinner', 'Available'),
    (8, '2023-07-30', 'Dinner', 'Available'),

    -- Branch C time slots
    (9, '2023-07-30', 'Breakfast', 'Available'),
    (10, '2023-07-30', 'Breakfast', 'Available'),
    (11, '2023-07-30', 'Breakfast', 'Available'),
    (12, '2023-07-30', 'Breakfast', 'Available'),

    (9, '2023-07-30', 'Lunch', 'Available'),
    (10, '2023-07-30', 'Lunch', 'Available'),
    (11, '2023-07-30', 'Lunch', 'Available'),
    (12, '2023-07-30', 'Lunch', 'Available'),

    (9, '2023-07-30', 'Dinner', 'Available'),
    (10, '2023-07-30', 'Dinner', 'Available'),
    (11, '2023-07-30', 'Dinner', 'Available'),
    (12, '2023-07-30', 'Dinner', 'Available'),

    -- Branch D time slots
    (13, '2023-07-30', 'Breakfast', 'Available'),
    (14, '2023-07-30', 'Breakfast', 'Available'),
    (15, '2023-07-30', 'Breakfast', 'Available'),
    (16, '2023-07-30', 'Breakfast', 'Available'),

    (13, '2023-07-30', 'Lunch', 'Available'),
    (14, '2023-07-30', 'Lunch', 'Available'),
    (15, '2023-07-30', 'Lunch', 'Available'),
    (16, '2023-07-30', 'Lunch', 'Available'),

    (13, '2023-07-30', 'Dinner', 'Available'),
    (14, '2023-07-30', 'Dinner', 'Available'),
    (15, '2023-07-30', 'Dinner', 'Available'),
    (16, '2023-07-30', 'Dinner', 'Available'),

    -- Branch E time slots
    (17, '2023-07-30', 'Breakfast', 'Available'),
    (18, '2023-07-30', 'Breakfast', 'Available'),
    (19, '2023-07-30', 'Breakfast', 'Available'),
    (20, '2023-07-30', 'Breakfast', 'Available'),

    (17, '2023-07-30', 'Lunch', 'Available'),
    (18, '2023-07-30', 'Lunch', 'Available'),
    (19, '2023-07-30', 'Lunch', 'Available'),
    (20, '2023-07-30', 'Lunch', 'Available'),

    (17, '2023-07-30', 'Dinner', 'Available'),
    (18, '2023-07-30', 'Dinner', 'Available'),
    (19, '2023-07-30', 'Dinner', 'Available'),
    (20, '2023-07-30', 'Dinner', 'Available');


    -- Prepare for sample booking data
    -- Assume the following are the user IDs and time slot IDs that will be used for simulation
DECLARE @userId1 INT = 1;
DECLARE @userId2 INT = 2;
DECLARE @userId3 INT = 3;

-- Simulate reservations for Branch A (RestaurantBranchId: 1) on 2023-07-30
-- User 1 booked the Breakfast time slot for Table 1
DECLARE @timeSlotId1 INT = (SELECT TOP 1 Id FROM TimeSlots WHERE ReservationDay = '2023-07-30' AND MealType = 'Breakfast' AND TableStatus = 'Available');
INSERT INTO Reservations (UserId, TimeSlotId, ReservationDate, ReservationStatus)
VALUES (@userId1, @timeSlotId1, '2023-07-30', 'Booked');

-- Simulate check-in for User 1 for the Breakfast time slot on Table 1
UPDATE TimeSlots
SET TableStatus = 'Occupied'
WHERE Id = @timeSlotId1;

-- Simulate check-out for User 1 for the Breakfast time slot on Table 1
UPDATE TimeSlots
SET TableStatus = 'Available'
WHERE Id = @timeSlotId1;

-- User 2 booked the Lunch time slot for Table 2
DECLARE @timeSlotId2 INT = (SELECT TOP 1 Id FROM TimeSlots WHERE ReservationDay = '2023-07-30' AND MealType = 'Lunch' AND TableStatus = 'Available');
INSERT INTO Reservations (UserId, TimeSlotId, ReservationDate, ReservationStatus)
VALUES (@userId2, @timeSlotId2, '2023-07-30', 'Booked');

-- Simulate check-in for User 2 for the Lunch time slot on Table 2
UPDATE TimeSlots
SET TableStatus = 'Occupied'
WHERE Id = @timeSlotId2;

-- Simulate check-out for User 2 for the Lunch time slot on Table 2
UPDATE TimeSlots
SET TableStatus = 'Available'
WHERE Id = @timeSlotId2;

-- User 3 booked the Dinner time slot for Table 3
DECLARE @timeSlotId3 INT = (SELECT TOP 1 Id FROM TimeSlots WHERE ReservationDay = '2023-07-30' AND MealType = 'Dinner' AND TableStatus = 'Available');
INSERT INTO Reservations (UserId, TimeSlotId, ReservationDate, ReservationStatus)
VALUES (@userId3, @timeSlotId3, '2023-07-30', 'Booked');

-- Simulate check-in for User 3 for the Dinner time slot on Table 3
UPDATE TimeSlots
SET TableStatus = 'Occupied'
WHERE Id = @timeSlotId3;

-- Simulate check-out for User 3 for the Dinner time slot on Table 3
UPDATE TimeSlots
SET TableStatus = 'Available'
WHERE Id = @timeSlotId3;


-- use this query to get data

SELECT        
TimeSlots.ReservationDay, 
DiningTables.TableName, 
TimeSlots.MealType, 
TimeSlots.TableStatus,
RestaurantBranches.Name, 
Users.FirstName, 
Users.LastName, 
Reservations.ReservationDate, 
Reservations.ReservationStatus
FROM            Users INNER JOIN
Reservations ON Users.Id = Reservations.UserId RIGHT OUTER JOIN
DiningTables INNER JOIN
TimeSlots ON DiningTables.Id = TimeSlots.DiningTableId INNER JOIN
RestaurantBranches ON DiningTables.RestaurantBranchId = RestaurantBranches.Id ON Reservations.TimeSlotId = TimeSlots.Id