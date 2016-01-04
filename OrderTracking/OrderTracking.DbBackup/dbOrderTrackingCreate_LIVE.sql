USE [OrderTracking]
GO
/****** Object:  Table [dbo].[tbl_Coordinates]    Script Date: 11/09/2009 09:33:29 ******/
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
/****** Object:  Table [dbo].[tbl_Log]    Script Date: 11/09/2009 09:33:30 ******/
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
/****** Object:  Table [dbo].[tbl_Status]    Script Date: 11/09/2009 09:33:30 ******/
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
/****** Object:  Table [dbo].[tbl_TrackerStatus]    Script Date: 11/09/2009 09:33:30 ******/
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
/****** Object:  Table [dbo].[tbl_Store]    Script Date: 11/09/2009 09:33:30 ******/
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
/****** Object:  Table [dbo].[tbl_Customer]    Script Date: 11/09/2009 09:33:29 ******/
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
/****** Object:  Table [dbo].[tbl_Account]    Script Date: 11/09/2009 09:33:29 ******/
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
/****** Object:  Table [dbo].[tbl_Driver]    Script Date: 11/09/2009 09:33:29 ******/
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
/****** Object:  Table [dbo].[tbl_Order]    Script Date: 11/09/2009 09:33:30 ******/
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
/****** Object:  Table [dbo].[tbl_Tracker]    Script Date: 11/09/2009 09:33:30 ******/
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
/****** Object:  Table [dbo].[tbl_OrderStatus]    Script Date: 11/09/2009 09:33:30 ******/
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
/****** Object:  Table [dbo].[tbl_Item]    Script Date: 11/09/2009 09:33:29 ******/
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
/****** Object:  Table [dbo].[tbl_DriverOrders]    Script Date: 11/09/2009 09:33:29 ******/
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
/****** Object:  Table [dbo].[tbl_CustomerOrders]    Script Date: 11/09/2009 09:33:29 ******/
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
/****** Object:  Default [DF_tbl_Log_Created]    Script Date: 11/09/2009 09:33:30 ******/
ALTER TABLE [dbo].[tbl_Log] ADD  CONSTRAINT [DF_tbl_Log_Created]  DEFAULT (getdate()) FOR [Created]
GO
/****** Object:  Default [DF_tbl_Order_Created]    Script Date: 11/09/2009 09:33:30 ******/
ALTER TABLE [dbo].[tbl_Order] ADD  CONSTRAINT [DF_tbl_Order_Created]  DEFAULT (getdate()) FOR [Created]
GO
/****** Object:  ForeignKey [FK_tbl_Account_tbl_Store]    Script Date: 11/09/2009 09:33:29 ******/
ALTER TABLE [dbo].[tbl_Account]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Account_tbl_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[tbl_Store] ([id])
GO
ALTER TABLE [dbo].[tbl_Account] CHECK CONSTRAINT [FK_tbl_Account_tbl_Store]
GO
/****** Object:  ForeignKey [FK_tbl_Customer_tbl_Coordinates]    Script Date: 11/09/2009 09:33:29 ******/
ALTER TABLE [dbo].[tbl_Customer]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Customer_tbl_Coordinates] FOREIGN KEY([Coordinates])
REFERENCES [dbo].[tbl_Coordinates] ([id])
GO
ALTER TABLE [dbo].[tbl_Customer] CHECK CONSTRAINT [FK_tbl_Customer_tbl_Coordinates]
GO
/****** Object:  ForeignKey [FK_tbl_CustomerOrders_tbl_Customer]    Script Date: 11/09/2009 09:33:29 ******/
ALTER TABLE [dbo].[tbl_CustomerOrders]  WITH CHECK ADD  CONSTRAINT [FK_tbl_CustomerOrders_tbl_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tbl_Customer] ([id])
GO
ALTER TABLE [dbo].[tbl_CustomerOrders] CHECK CONSTRAINT [FK_tbl_CustomerOrders_tbl_Customer]
GO
/****** Object:  ForeignKey [FK_tbl_CustomerOrders_tbl_Order]    Script Date: 11/09/2009 09:33:29 ******/
ALTER TABLE [dbo].[tbl_CustomerOrders]  WITH CHECK ADD  CONSTRAINT [FK_tbl_CustomerOrders_tbl_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tbl_Order] ([id])
GO
ALTER TABLE [dbo].[tbl_CustomerOrders] CHECK CONSTRAINT [FK_tbl_CustomerOrders_tbl_Order]
GO
/****** Object:  ForeignKey [FK_tbl_Driver_tbl_Store]    Script Date: 11/09/2009 09:33:29 ******/
ALTER TABLE [dbo].[tbl_Driver]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Driver_tbl_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[tbl_Store] ([id])
GO
ALTER TABLE [dbo].[tbl_Driver] CHECK CONSTRAINT [FK_tbl_Driver_tbl_Store]
GO
/****** Object:  ForeignKey [FK_tbl_DriverOrders_tbl_Driver]    Script Date: 11/09/2009 09:33:29 ******/
ALTER TABLE [dbo].[tbl_DriverOrders]  WITH CHECK ADD  CONSTRAINT [FK_tbl_DriverOrders_tbl_Driver] FOREIGN KEY([DriverId])
REFERENCES [dbo].[tbl_Driver] ([id])
GO
ALTER TABLE [dbo].[tbl_DriverOrders] CHECK CONSTRAINT [FK_tbl_DriverOrders_tbl_Driver]
GO
/****** Object:  ForeignKey [FK_tbl_DriverOrders_tbl_Order]    Script Date: 11/09/2009 09:33:29 ******/
ALTER TABLE [dbo].[tbl_DriverOrders]  WITH CHECK ADD  CONSTRAINT [FK_tbl_DriverOrders_tbl_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tbl_Order] ([id])
GO
ALTER TABLE [dbo].[tbl_DriverOrders] CHECK CONSTRAINT [FK_tbl_DriverOrders_tbl_Order]
GO
/****** Object:  ForeignKey [FK_tbl_Item_tbl_Order]    Script Date: 11/09/2009 09:33:29 ******/
ALTER TABLE [dbo].[tbl_Item]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Item_tbl_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tbl_Order] ([id])
GO
ALTER TABLE [dbo].[tbl_Item] CHECK CONSTRAINT [FK_tbl_Item_tbl_Order]
GO
/****** Object:  ForeignKey [FK_tbl_Order_tbl_Store]    Script Date: 11/09/2009 09:33:30 ******/
ALTER TABLE [dbo].[tbl_Order]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Order_tbl_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[tbl_Store] ([id])
GO
ALTER TABLE [dbo].[tbl_Order] CHECK CONSTRAINT [FK_tbl_Order_tbl_Store]
GO
/****** Object:  ForeignKey [FK_tbl_OrderStatus_tbl_Order]    Script Date: 11/09/2009 09:33:30 ******/
ALTER TABLE [dbo].[tbl_OrderStatus]  WITH CHECK ADD  CONSTRAINT [FK_tbl_OrderStatus_tbl_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tbl_Order] ([id])
GO
ALTER TABLE [dbo].[tbl_OrderStatus] CHECK CONSTRAINT [FK_tbl_OrderStatus_tbl_Order]
GO
/****** Object:  ForeignKey [FK_tbl_OrderStatus_tbl_Status]    Script Date: 11/09/2009 09:33:30 ******/
ALTER TABLE [dbo].[tbl_OrderStatus]  WITH CHECK ADD  CONSTRAINT [FK_tbl_OrderStatus_tbl_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[tbl_Status] ([id])
GO
ALTER TABLE [dbo].[tbl_OrderStatus] CHECK CONSTRAINT [FK_tbl_OrderStatus_tbl_Status]
GO
/****** Object:  ForeignKey [FK_tbl_Store_tbl_Coordinates]    Script Date: 11/09/2009 09:33:30 ******/
ALTER TABLE [dbo].[tbl_Store]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Store_tbl_Coordinates] FOREIGN KEY([Coordinates])
REFERENCES [dbo].[tbl_Coordinates] ([id])
GO
ALTER TABLE [dbo].[tbl_Store] CHECK CONSTRAINT [FK_tbl_Store_tbl_Coordinates]
GO
/****** Object:  ForeignKey [FK_tbl_Tracker_tbl_Coordinates]    Script Date: 11/09/2009 09:33:30 ******/
ALTER TABLE [dbo].[tbl_Tracker]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Tracker_tbl_Coordinates] FOREIGN KEY([Coordinates])
REFERENCES [dbo].[tbl_Coordinates] ([id])
GO
ALTER TABLE [dbo].[tbl_Tracker] CHECK CONSTRAINT [FK_tbl_Tracker_tbl_Coordinates]
GO
/****** Object:  ForeignKey [FK_tbl_Tracker_tbl_Driver]    Script Date: 11/09/2009 09:33:30 ******/
ALTER TABLE [dbo].[tbl_Tracker]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Tracker_tbl_Driver] FOREIGN KEY([DriverId])
REFERENCES [dbo].[tbl_Driver] ([id])
GO
ALTER TABLE [dbo].[tbl_Tracker] CHECK CONSTRAINT [FK_tbl_Tracker_tbl_Driver]
GO
/****** Object:  ForeignKey [FK_tbl_Tracker_tbl_Store]    Script Date: 11/09/2009 09:33:30 ******/
ALTER TABLE [dbo].[tbl_Tracker]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Tracker_tbl_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[tbl_Store] ([id])
GO
ALTER TABLE [dbo].[tbl_Tracker] CHECK CONSTRAINT [FK_tbl_Tracker_tbl_Store]
GO
/****** Object:  ForeignKey [FK_tbl_Tracker_tbl_TrackerStatus]    Script Date: 11/09/2009 09:33:30 ******/
ALTER TABLE [dbo].[tbl_Tracker]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Tracker_tbl_TrackerStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[tbl_TrackerStatus] ([id])
GO
ALTER TABLE [dbo].[tbl_Tracker] CHECK CONSTRAINT [FK_tbl_Tracker_tbl_TrackerStatus]
GO
