
ALTER DATABASE [OrderTracking] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
exec sp_dboption N'OrderTracking', N'single', N'false' 