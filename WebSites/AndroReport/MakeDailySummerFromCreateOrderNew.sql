/****** Script for SelectTopNRows command from SSMS  ******/
USE DailyReporting;

WITH AllCashedOrders
AS
(SELECT 
TheDate, 
nstoreid,
SUM(n_orderpricenet-n_vatcost) AS NetSales,
COUNT(n_orderpricenet) AS TotalOrders,
SUM(IIF(n_occasion=1,1,0)) AS CarryOutTotalOrders,
SUM(IIF(n_occasion=1,n_orderpricenet-n_vatcost,0)) AS CarryOutNetSales,
SUM(IIF(n_occasion=4,1,0))	AS DineInTotalOrders,
SUM(IIF(n_occasion=4,n_orderpricenet-n_vatcost,0)) AS DineInNetSales,
SUM(IIF(n_occasion=2,1,0)) AS DelTotalOrders,
SUM(IIF(n_occasion=2,n_orderpricenet-n_vatcost,0)) AS DelNetSales,
/*SUM(IIF(n_occasion=2 AND (n_orderdispatchedtime IS NOT NULL),DATEDIFF(second,n_orderplacedtime,n_orderdispatchedtime),0))/
SUM(IIF(n_occasion=2 AND (n_orderdispatchedtime IS NOT NULL),1,0)) AS AvgOutTheDoor,
SUM(IIF(n_occasion=2 AND (n_ordermadetime IS NOT NULL),DATEDIFF(second,n_orderplacedtime,n_ordermadetime),0))/
SUM(IIF(n_occasion=2 AND (n_ordermadetime IS NOT NULL),1,0)) AS AvgMake,
SUM(IIF(n_occasion=2 AND (n_ordercuttime IS NOT NULL),DATEDIFF(second,n_orderplacedtime,n_ordercuttime),0))/
SUM(IIF(n_occasion=2 AND (n_ordercuttime IS NOT NULL),1,0)) AS AvgDoorTime,
SUM(IIF(n_occasion=2 AND (n_orderdispatchedtime IS NOT NULL),DATEDIFF(second,n_ordercuttime,n_orderdispatchedtime),0))/
SUM(IIF(n_occasion=2 AND (n_orderdispatchedtime IS NOT NULL),1,0)) AS RackTime,*/
AVG(IIF(n_occasion=2 AND (n_orderdispatchedtime IS NOT NULL),DATEDIFF(second,n_orderplacedtime,n_orderdispatchedtime),NULL)) AS AvgOutTheDoor,
AVG(IIF(n_occasion=2 AND (n_ordermadetime IS NOT NULL),DATEDIFF(second,n_orderplacedtime,n_ordermadetime),NULL)) AS AvgMake,
AVG(IIF(n_occasion=2 AND (n_ordercuttime IS NOT NULL),n_totaltime,NULL)) AS AvgDoorTime,
AVG(IIF(n_occasion=2 AND (n_orderdispatchedtime IS NOT NULL),DATEDIFF(second,n_ordercuttime,n_orderdispatchedtime),NULL)) AS RackTime,
SUM(IIF(n_occasion=2 AND DATEDIFF(second,n_orderplacedtime,n_orderdispatchedtime)<30*60,1,0)) AS NumOrdersLT30Mins,
SUM(IIF(n_occasion=2 AND DATEDIFF(second,n_orderplacedtime,n_orderdispatchedtime)<45*60,1,0)) AS NumOrdersLT45Mins,
SUM(IIF(nstoreID>100000000000,n_orderpricenet,0)) AS OnlineTrans,
SUM(IIF(nstoreID>100000000000,n_orderpricenet-n_vatcost,0)) AS OnlineNetSales,
SUM(IIF(nstoreID>100000000000 AND n_occasion=2,n_orderpricenet,0)) AS OnlineDelTrans,
SUM(IIF(nstoreID>100000000000 AND n_occasion=2,n_orderpricenet-n_vatcost,0)) AS OnlineDelNetSales,
SUM(n_vatcost) AS TotSalesTax,
SUM(n_orderpricegross-n_orderpricenet) AS ManagerDiscounts,
SUM(n_totalmenuprice-n_orderpricegross) AS DealDiscount,
SUM(n_totalmenuprice) AS FullMenuPrice,
0 as TotCOS
FROM [dbo].[createorder] 
WHERE (n_statusflags=5) 
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

IdealCostOfSales
AS
(SELECT
P.d_date AS TheDate,
P.nstoreid as nstoreid,
SUM(P.n_amount*R.n_UOM/P.nCaseSize/R.n_UOM/10) AS nIdealCOS -- replace first n_UOM by reserved
FROM dbo.products AS P INNER JOIN Recipe AS R ON (P.n_productid=R.RecipeID --AND 
--P.nstoreid=R.nStoreID
)
GROUP BY P.d_date, P.nstoreid,P.n_type 
HAVING(P.n_type=3)
),

/*HourlyPayAndCustomerID
AS
(SELECT
S.thedate as TheDate,
S.nstoreid as nstoreid
SUM(IIF(S.n_payrate>999999,0, S.n_payrate)*S.n_minspaid/60) AS nCOLHourlyPay
FROM scheduling AS S INNER JOIN  employees AS E ON S.n*/

TotalPay
AS
(SELECT 
TheDate,
nstoreid,
SUM(n_delcom) AS TotPay,
SUM(IIF(n_type=2,n_minspaid*60,0)) AS LabourSeconds,
SUM(IIF(n_type=2,IIF(n_payrate>999999,1,n_payrate)*n_minspaid/60,0)) AS LabourCost
FROM scheduling
GROUP BY thedate, nstoreid),

AllOrders
AS
(SELECT 
COALESCE(AllCashedOrders.TheDate, CancelledOrders.TheDate) AS TheDate,
COALESCE(AllCashedOrders.nstoreid, CancelledOrders.nstoreid) AS nStoreId, 
nIdealCOS,
NULL AS nCOLHourlyPay,
TotPay, 
NetSales,
TotalOrders,DineInNetSales,DelNetSales,
CarryOutNetSales,DineInTotalOrders,DelTotalOrders,CarryOutTotalOrders,
TotSalesTax,ManagerDiscounts,DealDiscount,FullMenuPrice,
LabourSeconds,LabourCost,
ISNULL(TotalCancels,0) AS TotalCancels, ISNULL(valueCancels,0) AS valueCancels,NumOrdersLT30Mins,NumOrdersLT45Mins,
AvgMake,AvgOutTheDoor,AvgDoorTime,
OnlineNetSales, OnlineDelNetSales,OnlineTrans,OnlineDelTrans,
RackTime
FROM AllCashedOrders 
FULL JOIN CancelledOrders ON (AllCashedorders.TheDate=CancelledOrders.TheDate AND AllCashedOrders.nstoreid=CancelledOrders.nstoreid) 
LEFT JOIN IdealCostOfSales ON (COALESCE(AllCashedOrders.TheDate, CancelledOrders.TheDate)=IdealCostOfSales.TheDate 
AND COALESCE(AllCashedOrders.nstoreid, CancelledOrders.nstoreid)=IdealCostOfSales.nstoreid)
LEFT JOIN TotalPay ON (COALESCE(AllCashedOrders.TheDate, CancelledOrders.TheDate)=TotalPay.TheDate 
AND COALESCE(AllCashedOrders.nstoreid, CancelledOrders.nstoreid)=TotalPay.nstoreid)
)

SELECT * FROM AllOrders where (TheDate ='20131008' AND  nstoreid=209)
ORDER BY TheDate, nstoreid;




