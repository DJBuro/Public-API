USE [OrderTracking_Test]
GO

/****** Object:  Table [dbo].[tbl_SmsCredentials]    Script Date: 05/11/2010 13:47:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_SmsCredentials](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[SmsFrom] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbl_SmsCredentials] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[tbl_Apn]    Script Date: 05/11/2010 13:46:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Apn](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Provider] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Username] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
 CONSTRAINT [PK_tbl_Apn] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[tbl_CommandType]    Script Date: 05/11/2010 13:47:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_CommandType](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbl_CommandType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[tbl_TrackerType]    Script Date: 05/11/2010 13:51:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_TrackerType](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbl_TrackerType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[tbl_TrackerCommands]    Script Date: 05/11/2010 13:51:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_TrackerCommands](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Priority] [tinyint] NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Command] [nvarchar](150) NOT NULL,
	[TrackerTypeId] [bigint] NOT NULL,
	[CommandTypeId] [bigint] NOT NULL,
 CONSTRAINT [PK_tbl_TrackerCommands] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*Drop Tracker Table*/

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tbl_Tracker_tbl_Coordinates]') AND parent_object_id = OBJECT_ID(N'[dbo].[tbl_Tracker]'))
ALTER TABLE [dbo].[tbl_Tracker] DROP CONSTRAINT [FK_tbl_Tracker_tbl_Coordinates]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tbl_Tracker_tbl_Driver]') AND parent_object_id = OBJECT_ID(N'[dbo].[tbl_Tracker]'))
ALTER TABLE [dbo].[tbl_Tracker] DROP CONSTRAINT [FK_tbl_Tracker_tbl_Driver]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tbl_Tracker_tbl_Store]') AND parent_object_id = OBJECT_ID(N'[dbo].[tbl_Tracker]'))
ALTER TABLE [dbo].[tbl_Tracker] DROP CONSTRAINT [FK_tbl_Tracker_tbl_Store]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tbl_Tracker_tbl_TrackerStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[tbl_Tracker]'))
ALTER TABLE [dbo].[tbl_Tracker] DROP CONSTRAINT [FK_tbl_Tracker_tbl_TrackerStatus]
GO

USE [OrderTracking_Test]
GO

/****** Object:  Table [dbo].[tbl_Tracker]    Script Date: 05/11/2010 13:57:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_Tracker]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_Tracker]
GO

/****** Object:  Table [dbo].[tbl_Tracker]    Script Date: 05/11/2010 13:58:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Tracker](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[DriverId] [bigint] NULL,
	[StoreId] [bigint] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Status] [bigint] NOT NULL,
	[BatteryLevel] [int] NULL,
	[Coordinates] [bigint] NULL,
	[IMEI] [bigint] NOT NULL,
	[SerialNumber] [nvarchar](50) NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[TypeId] [bigint] NOT NULL,
	[ApnId] [bigint] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_Tracker] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO






ALTER TABLE [dbo].[tbl_Tracker]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Tracker_tbl_Apn] FOREIGN KEY([ApnId])
REFERENCES [dbo].[tbl_Apn] ([id])
GO

ALTER TABLE [dbo].[tbl_Tracker] CHECK CONSTRAINT [FK_tbl_Tracker_tbl_Apn]
GO

ALTER TABLE [dbo].[tbl_Tracker]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Tracker_tbl_Coordinates] FOREIGN KEY([Coordinates])
REFERENCES [dbo].[tbl_Coordinates] ([id])
GO

ALTER TABLE [dbo].[tbl_Tracker] CHECK CONSTRAINT [FK_tbl_Tracker_tbl_Coordinates]
GO

ALTER TABLE [dbo].[tbl_Tracker]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Tracker_tbl_Driver] FOREIGN KEY([DriverId])
REFERENCES [dbo].[tbl_Driver] ([id])
GO

ALTER TABLE [dbo].[tbl_Tracker] CHECK CONSTRAINT [FK_tbl_Tracker_tbl_Driver]
GO

ALTER TABLE [dbo].[tbl_Tracker]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Tracker_tbl_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[tbl_Store] ([id])
GO

ALTER TABLE [dbo].[tbl_Tracker] CHECK CONSTRAINT [FK_tbl_Tracker_tbl_Store]
GO

ALTER TABLE [dbo].[tbl_Tracker]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Tracker_tbl_TrackerStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[tbl_TrackerStatus] ([id])
GO

ALTER TABLE [dbo].[tbl_Tracker] CHECK CONSTRAINT [FK_tbl_Tracker_tbl_TrackerStatus]
GO

ALTER TABLE [dbo].[tbl_Tracker]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Tracker_tbl_TrackerType] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tbl_TrackerType] ([id])
GO

ALTER TABLE [dbo].[tbl_Tracker] CHECK CONSTRAINT [FK_tbl_Tracker_tbl_TrackerType]
GO

