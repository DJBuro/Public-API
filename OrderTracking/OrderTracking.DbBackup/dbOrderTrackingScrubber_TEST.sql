

DELETE FROM [tbl_Log]
DELETE FROM [tbl_Account]
DELETE FROM [tbl_Tracker]
DELETE FROM [tbl_CustomerOrders]
DELETE FROM [tbl_Customer]
DELETE FROM [tbl_Item]
DELETE FROM [tbl_OrderStatus]
DELETE FROM [tbl_DriverOrders]
DELETE FROM [tbl_Driver]
DELETE FROM [tbl_OrderStatus]
DELETE FROM [tbl_Order]
DELETE FROM [tbl_Store]
DELETE FROM [tbl_Coordinates]

      
dbcc checkident ([tbl_Account], reseed, 0)
dbcc checkident ([tbl_Tracker], reseed, 0)
dbcc checkident ([tbl_Customer], reseed, 0)
dbcc checkident ([tbl_Log], reseed, 0)
dbcc checkident ([tbl_CustomerOrders], reseed, 0)
dbcc checkident ([tbl_Item], reseed, 0)
dbcc checkident ([tbl_OrderStatus], reseed, 0)
dbcc checkident ([tbl_DriverOrders], reseed, 0)
dbcc checkident ([tbl_Driver], reseed, 0)
dbcc checkident ([tbl_OrderStatus], reseed, 0)
dbcc checkident ([tbl_Store], reseed, 0)
dbcc checkident ([tbl_Coordinates], reseed, 0)

dbcc checkident ([tbl_Coordinates], reseed, 0)


Declare @DriverId BIGINT
Declare @CoordId BIGINT
Declare @StoreId BIGINT
/*
INSERT INTO [tbl_Driver]
           ([StoreId]
           ,[Name]
           ,[ExternalDriverId]
           ,[Vehicle])
     VALUES
           (1
           ,'Driver Bob'
           ,'driver009'
           ,'velosolex')

Set @DriverId = (SELECT SCOPE_IDENTITY())

INSERT INTO [tbl_Tracker]
           ([DriverId]
           ,[StoreId]
           ,[Name]
           ,[Longitude]
           ,[Latitude]
           ,[Status])
     VALUES
           (@DriverId
           ,1
           ,'TRACKER3'
           ,NULL
           ,NULL
           ,1)
*/


--INDIA DINING
INSERT INTO [OrderTracking_Test].[dbo].[tbl_Coordinates]
           ([Longitude]
           ,[Latitude])
     VALUES
           ('-0.077837'
           ,'51.378698')

Set @CoordId = (SELECT SCOPE_IDENTITY())

INSERT INTO [OrderTracking_Test].[dbo].[tbl_Store]
           ([ExternalStoreId]
           ,[Name]
           ,[Coordinates]
           ,[DeliveryRadius])
     VALUES
           ('267'
           ,'Papa Johns Croydon East '
           ,@CoordId
           ,10)

Set @StoreId = (SELECT SCOPE_IDENTITY())

INSERT INTO [OrderTracking_Test].[dbo].[tbl_Account]
           ([UserName]
           ,[Password]
           ,[StoreId]
           ,[GpsEnabled])
     VALUES
           ('MONITOR267'
           ,'Pass'
           ,@StoreId
           ,1)


--BOX PIZZA
INSERT INTO [OrderTracking_Test].[dbo].[tbl_Coordinates]
           ([Longitude]
           ,[Latitude])
     VALUES
           ('-1.5648573637008667'
           ,'53.802284240722656')

Set @CoordId = (SELECT SCOPE_IDENTITY())

INSERT INTO [OrderTracking_Test].[dbo].[tbl_Store]
           ([ExternalStoreId]
           ,[Name]
           ,[Coordinates]
           ,[DeliveryRadius])
     VALUES
           ('362'
           ,'Box Pizza'
           ,@CoordId
           ,10)

Set @StoreId = (SELECT SCOPE_IDENTITY())

INSERT INTO [OrderTracking_Test].[dbo].[tbl_Account]
           ([UserName]
           ,[Password]
           ,[StoreId]
           ,[GpsEnabled])
     VALUES
           ('MONITOR362'
           ,'Pass'
           ,@StoreId
           ,1)
           
--RED PLANET           
INSERT INTO [OrderTracking_Test].[dbo].[tbl_Coordinates]
           ([Longitude]
           ,[Latitude])
     VALUES
           ('-0.087859'
           ,'51.495130')

Set @CoordId = (SELECT SCOPE_IDENTITY())


INSERT INTO [OrderTracking_Test].[dbo].[tbl_Store]
           ([ExternalStoreId]
           ,[Name]
           ,[Coordinates]
           ,[DeliveryRadius])
     VALUES
           ('623'
           ,'Red Planet'
           ,@CoordId
           ,7)

Set @StoreId = (SELECT SCOPE_IDENTITY())

INSERT INTO [OrderTracking_Test].[dbo].[tbl_Account]
           ([UserName]
           ,[Password]
           ,[StoreId]
           ,[GpsEnabled])
     VALUES
           ('MONITOR623'
           ,'Pass'
           ,@StoreId
           ,0)

--Herbies Slough
INSERT INTO [OrderTracking_Test].[dbo].[tbl_Coordinates]
           ([Longitude]
           ,[Latitude])
     VALUES
           ('-0.6149572'
           ,'51.52103')

Set @CoordId = (SELECT SCOPE_IDENTITY())


INSERT INTO [OrderTracking_Test].[dbo].[tbl_Store]
           ([ExternalStoreId]
           ,[Name]
           ,[Coordinates]
           ,[DeliveryRadius])
     VALUES
           ('4'
           ,'Herbies Slough'
           ,@CoordId
           ,7)

Set @StoreId = (SELECT SCOPE_IDENTITY())

INSERT INTO [OrderTracking_Test].[dbo].[tbl_Account]
           ([UserName]
           ,[Password]
           ,[StoreId]
           ,[GpsEnabled])
     VALUES
           ('MONITOR4'
           ,'Pass'
           ,@StoreId
           ,0)
           
/*
long/lat
1	-0.077837	51.378698
2	-1.5648573637008667	53.802284240722656
*/