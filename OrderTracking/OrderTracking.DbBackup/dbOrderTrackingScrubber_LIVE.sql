/*
USE [master]
GO

CREATE DATABASE [OrderTracking] ON  PRIMARY 
( NAME = N'OrderTracking', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.TESTSQL2008\MSSQL\DATA\OrderTracking.mdf' , SIZE = 7168KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'OrderTracking_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.TESTSQL2008\MSSQL\DATA\OrderTracking.ldf' , SIZE = 102144KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [OrderTracking] SET COMPATIBILITY_LEVEL = 90
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OrderTracking].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO

ALTER DATABASE [OrderTracking] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [OrderTracking] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [OrderTracking] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [OrderTracking] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [OrderTracking] SET ARITHABORT OFF 
GO

ALTER DATABASE [OrderTracking] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [OrderTracking] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [OrderTracking] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [OrderTracking] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [OrderTracking] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [OrderTracking] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [OrderTracking] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [OrderTracking] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [OrderTracking] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [OrderTracking] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [OrderTracking] SET  DISABLE_BROKER 
GO

ALTER DATABASE [OrderTracking] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [OrderTracking] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [OrderTracking] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [OrderTracking] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [OrderTracking] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [OrderTracking] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [OrderTracking] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [OrderTracking] SET  READ_WRITE 
GO

ALTER DATABASE [OrderTracking] SET RECOVERY FULL 
GO

ALTER DATABASE [OrderTracking] SET  MULTI_USER 
GO

ALTER DATABASE [OrderTracking] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [OrderTracking] SET DB_CHAINING OFF 
GO
*/
USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_Log]    Script Date: 02/08/2010 13:40:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Log](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[StoreId] [nvarchar](50) NULL,
	[Severity] [nvarchar](50) NULL,
	[Message] [nvarchar](500) NULL,
	[Method] [nvarchar](200) NULL,
	[Source] [nvarchar](50) NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_tbl_Log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_Log] ADD  CONSTRAINT [DF_tbl_Log_Created]  DEFAULT (getdate()) FOR [Created]
GO



USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_Coordinates]    Script Date: 02/08/2010 13:39:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Coordinates](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
 CONSTRAINT [PK_tbl_Coordinates] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO




USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_TrackerStatus]    Script Date: 02/08/2010 13:41:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_TrackerStatus](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbl_TrackerStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[tbl_Status]    Script Date: 02/08/2010 13:42:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Status](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbl_Status] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_Store]    Script Date: 02/08/2010 13:43:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Store](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[ExternalStoreId] [nvarchar](50) NULL,
	[Name] [nvarchar](100) NULL,
	[Coordinates] [bigint] NOT NULL,
	[DeliveryRadius] [smallint] NOT NULL,
 CONSTRAINT [PK_tbl_Store] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_Store]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Store_tbl_Coordinates] FOREIGN KEY([Coordinates])
REFERENCES [dbo].[tbl_Coordinates] ([id])
GO

ALTER TABLE [dbo].[tbl_Store] CHECK CONSTRAINT [FK_tbl_Store_tbl_Coordinates]
GO

USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_Order]    Script Date: 02/08/2010 13:43:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Order](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[ExternalOrderId] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[StoreId] [bigint] NOT NULL,
	[TicketNumber] [nvarchar](50) NULL,
	[ExtraInformation] [nvarchar](100) NULL,
	[ProximityDelivered] [smalldatetime] NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_tbl_Order] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_Order]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Order_tbl_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[tbl_Store] ([id])
GO

ALTER TABLE [dbo].[tbl_Order] CHECK CONSTRAINT [FK_tbl_Order_tbl_Store]
GO

ALTER TABLE [dbo].[tbl_Order] ADD  CONSTRAINT [DF_tbl_Order_Created]  DEFAULT (getdate()) FOR [Created]
GO


USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_OrderStatus]    Script Date: 02/08/2010 13:44:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_OrderStatus](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[StatusId] [bigint] NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[Processor] [nvarchar](50) NULL,
	[Time] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_tbl_OrderStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_OrderStatus]  WITH CHECK ADD  CONSTRAINT [FK_tbl_OrderStatus_tbl_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tbl_Order] ([id])
GO

ALTER TABLE [dbo].[tbl_OrderStatus] CHECK CONSTRAINT [FK_tbl_OrderStatus_tbl_Order]
GO

ALTER TABLE [dbo].[tbl_OrderStatus]  WITH CHECK ADD  CONSTRAINT [FK_tbl_OrderStatus_tbl_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[tbl_Status] ([id])
GO

ALTER TABLE [dbo].[tbl_OrderStatus] CHECK CONSTRAINT [FK_tbl_OrderStatus_tbl_Status]
GO

USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_Item]    Script Date: 02/08/2010 13:42:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Item](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbl_Item] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_Item]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Item_tbl_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tbl_Order] ([id])
GO

ALTER TABLE [dbo].[tbl_Item] CHECK CONSTRAINT [FK_tbl_Item_tbl_Order]
GO

USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_Customer]    Script Date: 02/08/2010 13:45:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Customer](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[ExternalId] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Credentials] [nvarchar](20) NOT NULL,
	[Coordinates] [bigint] NULL,
 CONSTRAINT [PK_tbl_Customer] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_Customer]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Customer_tbl_Coordinates] FOREIGN KEY([Coordinates])
REFERENCES [dbo].[tbl_Coordinates] ([id])
GO

ALTER TABLE [dbo].[tbl_Customer] CHECK CONSTRAINT [FK_tbl_Customer_tbl_Coordinates]
GO

USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_CustomerOrders]    Script Date: 02/08/2010 13:45:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_CustomerOrders](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[CustomerId] [bigint] NOT NULL,
 CONSTRAINT [PK_tbl_CustomerOrders] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_CustomerOrders]  WITH CHECK ADD  CONSTRAINT [FK_tbl_CustomerOrders_tbl_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tbl_Customer] ([id])
GO

ALTER TABLE [dbo].[tbl_CustomerOrders] CHECK CONSTRAINT [FK_tbl_CustomerOrders_tbl_Customer]
GO

ALTER TABLE [dbo].[tbl_CustomerOrders]  WITH CHECK ADD  CONSTRAINT [FK_tbl_CustomerOrders_tbl_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tbl_Order] ([id])
GO

ALTER TABLE [dbo].[tbl_CustomerOrders] CHECK CONSTRAINT [FK_tbl_CustomerOrders_tbl_Order]
GO

USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_Driver]    Script Date: 02/08/2010 13:46:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Driver](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[StoreId] [bigint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ExternalDriverId] [nvarchar](50) NOT NULL,
	[Vehicle] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbl_Driver] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_Driver]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Driver_tbl_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[tbl_Store] ([id])
GO

ALTER TABLE [dbo].[tbl_Driver] CHECK CONSTRAINT [FK_tbl_Driver_tbl_Store]
GO


USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_Tracker]    Script Date: 02/08/2010 13:47:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Tracker](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[DriverId] [bigint] NULL,
	[StoreId] [bigint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Status] [bigint] NOT NULL,
	[Coordinates] [bigint] NULL,
 CONSTRAINT [PK_tbl_Tracker] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

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


USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_DriverOrders]    Script Date: 02/08/2010 13:47:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_DriverOrders](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[DriverId] [bigint] NOT NULL,
 CONSTRAINT [PK_tbl_DriverOrders] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_DriverOrders]  WITH CHECK ADD  CONSTRAINT [FK_tbl_DriverOrders_tbl_Driver] FOREIGN KEY([DriverId])
REFERENCES [dbo].[tbl_Driver] ([id])
GO

ALTER TABLE [dbo].[tbl_DriverOrders] CHECK CONSTRAINT [FK_tbl_DriverOrders_tbl_Driver]
GO

ALTER TABLE [dbo].[tbl_DriverOrders]  WITH CHECK ADD  CONSTRAINT [FK_tbl_DriverOrders_tbl_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tbl_Order] ([id])
GO

ALTER TABLE [dbo].[tbl_DriverOrders] CHECK CONSTRAINT [FK_tbl_DriverOrders_tbl_Order]
GO

USE [OrderTracking]
GO

/****** Object:  Table [dbo].[tbl_Account]    Script Date: 02/08/2010 13:41:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Account](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[StoreId] [bigint] NOT NULL,
	[GpsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_Account] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbl_Account]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Account_tbl_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[tbl_Store] ([id])
GO

ALTER TABLE [dbo].[tbl_Account] CHECK CONSTRAINT [FK_tbl_Account_tbl_Store]
GO

USE [OrderTracking]
GO


/*Populate the values*/
/*
1	Order Taken
2	In Oven
3	Cut (Ready for delivery)
4	Out for delivery
5	Cashed Off
6	Cancelled

*/
INSERT INTO [OrderTracking].[dbo].[tbl_Status]
           ([Name])
     VALUES
           ('Order taken')
GO
INSERT INTO [OrderTracking].[dbo].[tbl_Status]
           ([Name])
     VALUES
           ('In oven')
GO
INSERT INTO [OrderTracking].[dbo].[tbl_Status]
           ([Name])
     VALUES
           ('Being prepared')
GO
INSERT INTO [OrderTracking].[dbo].[tbl_Status]
           ([Name])
     VALUES
           ('Out for delivery')
GO
INSERT INTO [OrderTracking].[dbo].[tbl_Status]
           ([Name])
     VALUES
           ('Cashed off')
GO
INSERT INTO [OrderTracking].[dbo].[tbl_Status]
           ([Name])
     VALUES
           ('Cancelled')
GO

/*
id	Name
1	Alive
2	Offline
3	Unknown
*/

INSERT INTO [OrderTracking].[dbo].[tbl_TrackerStatus]
           ([Name])
     VALUES
           ('Alive')
GO

INSERT INTO [OrderTracking].[dbo].[tbl_TrackerStatus]
           ([Name])
     VALUES
           ('Offline')
GO

INSERT INTO [OrderTracking].[dbo].[tbl_TrackerStatus]
           ([Name])
     VALUES
           ('Unknown')
GO