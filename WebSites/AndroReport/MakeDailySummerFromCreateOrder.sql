/****** Script for SelectTopNRows command from SSMS  ******/
USE DailyReporting;

WITH AllCashedOrders
AS
(SELECT 
TheDate, 
nstoreid,
SUM(n_orderpricenet-n_vatcost) AS NetSales,
COUNT(n_orderpricenet) as TotalOrders,
SUM(n_vatcost) AS TotSalesTax,
SUM(n_orderpricegross-n_orderpricenet) AS ManagerDiscounts,
SUM(n_totalmenuprice-n_orderpricegross) AS DealDiscount,
SUM(n_totalmenuprice) AS FullMenuPrice
FROM [dbo].[createorder] 
WHERE (n_statusflags=5) 
GROUP BY TheDate,nstoreid),

CarryOutOrders
AS
(SELECT
 TheDate, 
nstoreid,
SUM(n_orderpricenet-n_vatcost) AS CarryOutNetSales,
COUNT(n_orderpricenet) as CarryOutTotalOrders
FROM [dbo].[createorder] 
WHERE (n_statusflags=5 AND n_occasion=1) 
GROUP BY TheDate,nstoreid),

DineInOrders
AS
(SELECT 
 TheDate, 
nstoreid,
SUM(n_orderpricenet-n_vatcost) AS DineInNetSales,
COUNT(n_orderpricenet) as DineInTotalOrders
FROM [dbo].[createorder] 
WHERE (n_statusflags=5 AND n_occasion=4) 
GROUP BY TheDate,nstoreid),

DeliveryOrders
AS
(SELECT 
 TheDate, 
nstoreid,
SUM(n_orderpricenet-n_vatcost) AS DelNetSales,
COUNT(n_orderpricenet) as DelTotalOrders,
SUM(DATEDIFF(second,n_orderplacedtime,n_orderdispatchedtime))/COUNT(n_orderdispatchedtime) AS AvgOutTheDoor,
SUM(IIF(DATEDIFF(second,n_orderplacedtime,n_orderdispatchedtime)<30*60,1,0)) AS NumOrdersLT30Mins,
SUM(IIF(DATEDIFF(second,n_orderplacedtime,n_orderdispatchedtime)<45*60,1,0)) AS NumOrdersLT45Mins,
SUM(DATEDIFF(second,n_orderplacedtime,n_ordermadetime))/COUNT(n_ordermadetime) AS AvgMake,
SUM(DATEDIFF(second,n_orderplacedtime,n_ordercuttime))/COUNT(n_ordercuttime) AS AvgDoorTime
FROM [dbo].[createorder] 
WHERE (n_statusflags=5 AND n_occasion=2) 
GROUP BY TheDate,nstoreid),

CancelledOrders
AS
(SELECT 
TheDate, 
nstoreid,
SUM(n_orderpricenet-n_vatcost) AS valueCancels,
COUNT(n_orderpricenet) AS TotalCancels
FROM [dbo].[createorder] 
WHERE (n_statusflags=6) 
GROUP BY TheDate,nstoreid),

AllOrders
AS
(SELECT 
COALESCE(AllCashedOrders.TheDate, CancelledOrders.TheDate) AS TheDate,
COALESCE(AllCashedOrders.nstoreid, CancelledOrders.nstoreid) as nStoreId, 
NetSales,
TotalOrders,
DineInNetSales,
DelNetSales,
CarryOutNetSales,
DineInTotalOrders,
DelTotalOrders,
CarryOutTotalOrders,
AvgMake,
AvgDoorTime,
AvgOutTheDoor,
NumOrdersLT30Mins,
TotalCancels,
valueCancels
FROM AllCashedOrders 
LEFT JOIN CarryOutOrders ON (AllCashedorders.TheDate=CarryOutOrders.TheDate AND AllCashedOrders.nstoreid=CarryoutOrders.nstoreid)
LEFT JOIN DeliveryOrders ON (AllCashedorders.TheDate=DeliveryOrders.TheDate AND AllCashedOrders.nstoreid=DeliveryOrders.nstoreid)
LEFT JOIN DineInOrders ON (AllCashedorders.TheDate=DineInOrders.TheDate AND AllCashedOrders.nstoreid=DineInOrders.nstoreid)
FULL JOIN CancelledOrders ON (AllCashedorders.TheDate=CancelledOrders.TheDate AND AllCashedOrders.nstoreid=CancelledOrders.nstoreid) 

WHERE (AllCashedOrders.TheDate='20130705' AND AllCashedOrders.nstoreid=1186)
ORDER BY TheDate, nStoreId),

IdealCostOfSales
AS
(SELECT
P.d_date AS TheDate,
SUM(P.n_amount*R.n_UOM/P.nCaseSize/R.n_UOM/10) AS nIdealCOS -- replace first n_UOM by reserved
FROM dbo.products AS P INNER JOIN Recipe AS R ON (P.n_productid=R.RecipeID AND P.nstoreid=R.nStoreID)
GROUP BY P.d_date, P.n_type 
HAVING(P.n_type=3)
),

/*HourlyPayAndCustomerID
AS
(SELECT
S.thedate as TheDate,
SUM(IIF(S.n_payrate>999999,0, S.n_payrate)*S.n_minspaid/60) AS nCOLHourlyPay
FROM scheduling AS S INNER JOIN  employees AS E ON S.n*/

TotalPay
AS
(SELECT 
TheDate,
SUM(n_delcom) AS TotPay,
SUM(IIF(n_type=2,n_minspaid*60,0)) AS LabourSeconds,
SUM(IIF(n_type=2,IIF(n_payrate>999999,1,n-payrate)*n_minspaid/60,0)) AS LabourCost
FROM scheduling
GROUP BY thedate, nstoreid),


