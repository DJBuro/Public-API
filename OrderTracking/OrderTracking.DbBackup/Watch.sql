
SELECT     tbl_Customer.id, tbl_Customer.ExternalId, tbl_Customer.Name, tbl_Customer.Credentials, tbl_Customer.Coordinates, tbl_Order.ExternalOrderId, 
                      tbl_OrderStatus.StatusId, tbl_OrderStatus.Processor, tbl_OrderStatus.Time AS StatusTime, tbl_Driver.Name AS DriverName, 
                      tbl_Order.ProximityDelivered, tbl_Order.Created
FROM         tbl_Driver INNER JOIN
                      tbl_DriverOrders ON tbl_Driver.id = tbl_DriverOrders.DriverId RIGHT OUTER JOIN
                      tbl_Customer INNER JOIN
                      tbl_CustomerOrders ON tbl_Customer.id = tbl_CustomerOrders.CustomerId INNER JOIN
                      tbl_Order ON tbl_CustomerOrders.OrderId = tbl_Order.id INNER JOIN
                      tbl_OrderStatus ON tbl_Order.id = tbl_OrderStatus.OrderId INNER JOIN
                      tbl_Store ON tbl_Order.StoreId = tbl_Store.id ON tbl_DriverOrders.OrderId = tbl_Order.id
WHERE     (tbl_Store.id = 2)
ORDER BY tbl_Customer.id DESC


SELECT     tbl_Driver.ExternalDriverId, tbl_Driver.Name, tbl_Tracker.Name AS TrackerName
FROM         tbl_Driver LEFT OUTER JOIN
                      tbl_Tracker ON tbl_Driver.id = tbl_Tracker.DriverId
WHERE     (tbl_Driver.StoreId = 2)


SELECT TOP 1000 [id]
      ,[StoreId]
      ,[Severity]
      ,[Message]
      ,[Method]
      ,[Source]
      ,[Created]
  FROM [OrderTracking_Test].[dbo].[tbl_Log] 
    where [StoreId] = '362' 
    and (DATEDIFF(d,[Created],GETDATE())) < 1
    order by id desc
  
  
  
  

