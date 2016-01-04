DELETE FROM [tbl_TrackerCommands]
DELETE FROM [tbl_TrackerType]
DELETE FROM [tbl_CommandType]
DELETE FROM [tbl_SmsCredentials]
DELETE FROM [tbl_Apn]
     
dbcc checkident ([tbl_Apn], reseed, 0)     
dbcc checkident ([tbl_TrackerType], reseed, 0)
dbcc checkident ([tbl_CommandType], reseed, 0)
dbcc checkident ([tbl_TrackerCommands], reseed, 0)
dbcc checkident ([tbl_SmsCredentials], reseed, 0)

Declare @trackerTypeId BIGINT
Declare @commandTypeId BIGINT

INSERT INTO [dbo].[tbl_Apn]
           ([Provider]
           ,[Name]
           ,[Username]
           ,[Password])
     VALUES
           ('UK Vodaphone (contract)'
           ,'internet'
           ,'web'
           ,'webs')

INSERT INTO [dbo].[tbl_TrackerType]
           ([Name])
     VALUES
           ('gt30')

Set @trackerTypeId = (SELECT SCOPE_IDENTITY())

INSERT INTO [dbo].[tbl_CommandType]
           ([Name]
           ,[Description])
     VALUES
           ('Setup'
           ,'Setup the tracker')


Set @commandTypeId = (SELECT SCOPE_IDENTITY())


INSERT INTO [dbo].[tbl_TrackerCommands]
           ([Priority]
           ,[Name]
           ,[Command]
           ,[TrackerTypeId]
           ,[CommandTypeId])
     VALUES
           (1
           ,'Set Device Id'
           ,'W000000,010,{deviceId}'
           ,@trackerTypeId
           ,@commandTypeId)


INSERT INTO [dbo].[tbl_TrackerCommands]
           ([Priority]
           ,[Name]
           ,[Command]
           ,[TrackerTypeId]
           ,[CommandTypeId])
     VALUES
           (2
           ,'Set APN'
           ,'W000000,011,{apnName},{apnUserName},{apnPassword},8500'
           ,@trackerTypeId
           ,@commandTypeId)


INSERT INTO [dbo].[tbl_TrackerCommands]
           ([Priority]
           ,[Name]
           ,[Command]
           ,[TrackerTypeId]
           ,[CommandTypeId])
     VALUES
           (3
           ,'Set IP'
           ,'W000000,012,94.236.121.24,8500'
           ,@trackerTypeId
           ,@commandTypeId)


INSERT INTO [dbo].[tbl_TrackerCommands]
           ([Priority]
           ,[Name]
           ,[Command]
           ,[TrackerTypeId]
           ,[CommandTypeId])
     VALUES
           (4
           ,'Open TCP'
           ,'W000000,013,1'
           ,@trackerTypeId
           ,@commandTypeId)


INSERT INTO [dbo].[tbl_TrackerCommands]
           ([Priority]
           ,[Name]
           ,[Command]
           ,[TrackerTypeId]
           ,[CommandTypeId])
     VALUES
           (5
           ,'Set timer'
           ,'W000000,014,00001'
           ,@trackerTypeId
           ,@commandTypeId)


INSERT INTO [dbo].[tbl_SmsCredentials]
           ([Username]
           ,[Password]
           ,[SmsFrom])
     VALUES
           ('sck02411'
           ,'QLQXFS'
           ,'+447949483082')
GO

