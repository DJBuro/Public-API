USE [GpsGateServer]
GO

/****** Object:  Table [dbo].[gate_event_channel]    Script Date: 05/10/2010 16:16:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gate_event_channel]') AND type in (N'U'))
DROP TABLE [dbo].[gate_event_channel_groups]
GO


USE [GpsGateServer]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_geocoder_cache_geocoder_provider]') AND parent_object_id = OBJECT_ID(N'[dbo].[geocoder_cache]'))
ALTER TABLE [dbo].[geocoder_cache] DROP CONSTRAINT [FK_geocoder_cache_geocoder_provider]
GO

USE [GpsGateServer]
GO

/****** Object:  Table [dbo].[geocoder_cache]    Script Date: 05/10/2010 16:17:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[geocoder_cache]') AND type in (N'U'))
DROP TABLE [dbo].[geocoder_cache]
GO

USE [GpsGateServer]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_group_referrer_groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[group_referrer]'))
ALTER TABLE [dbo].[group_referrer] DROP CONSTRAINT [FK_group_referrer_groups]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_group_referrer_referrer]') AND parent_object_id = OBJECT_ID(N'[dbo].[group_referrer]'))
ALTER TABLE [dbo].[group_referrer] DROP CONSTRAINT [FK_group_referrer_referrer]
GO

USE [GpsGateServer]
GO

/****** Object:  Table [dbo].[group_referrer]    Script Date: 05/10/2010 16:18:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[group_referrer]') AND type in (N'U'))
DROP TABLE [dbo].[group_referrer]
GO

USE [GpsGateServer]
GO

/****** Object:  Table [dbo].[logs]    Script Date: 05/10/2010 16:18:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[logs]') AND type in (N'U'))
DROP TABLE [dbo].[logs]
GO


USE [GpsGateServer]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_map_lib_map]') AND parent_object_id = OBJECT_ID(N'[dbo].[map_lib_map_data]'))
ALTER TABLE [dbo].[map_lib_map_data] DROP CONSTRAINT [FK_map_lib_map]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_map_lib_map_data]') AND parent_object_id = OBJECT_ID(N'[dbo].[map_lib_map_data]'))
ALTER TABLE [dbo].[map_lib_map_data] DROP CONSTRAINT [FK_map_lib_map_data]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__map_lib_m__map_l__24285DB4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[map_lib_map_data] DROP CONSTRAINT [DF__map_lib_m__map_l__24285DB4]
END

GO

USE [GpsGateServer]
GO

/****** Object:  Table [dbo].[map_lib_map_data]    Script Date: 05/10/2010 16:19:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[map_lib_map_data]') AND type in (N'U'))
DROP TABLE [dbo].[map_lib_map_data]
GO

USE [GpsGateServer]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_usage_statistic_data]') AND parent_object_id = OBJECT_ID(N'[dbo].[usage_statistic]'))
ALTER TABLE [dbo].[usage_statistic] DROP CONSTRAINT [FK_usage_statistic_data]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_usage_statistic_session]') AND parent_object_id = OBJECT_ID(N'[dbo].[usage_statistic]'))
ALTER TABLE [dbo].[usage_statistic] DROP CONSTRAINT [FK_usage_statistic_session]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_usage_statistic_stat_key]') AND parent_object_id = OBJECT_ID(N'[dbo].[usage_statistic]'))
ALTER TABLE [dbo].[usage_statistic] DROP CONSTRAINT [FK_usage_statistic_stat_key]
GO

USE [GpsGateServer]
GO

/****** Object:  Table [dbo].[usage_statistic]    Script Date: 05/10/2010 16:19:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usage_statistic]') AND type in (N'U'))
DROP TABLE [dbo].[usage_statistic]
GO


