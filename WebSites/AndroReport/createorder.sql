/****** Script for SelectTopNRows command from SSMS  ******/
USE DailyReporting;
SELECT TOP 1000 
thedate,
      [nstoreid]
      ,[n_ordernum]
      ,[n_orderpricegross]
      ,[n_orderpricenet]
      ,[n_orderplacedtime]
      ,[n_ordermadetime],
	  DATEDIFF(second,[n_orderplacedtime],[n_ordermadetime]) as MakeTime,
	  (SELECT SUM(DATEDIFF(second,[n_orderplacedtime],[n_ordermadetime])) from dbo.createorder where thedate='20130705' and nstoreid=1186 group by thedate, nstoreid) as SUMM
      ,[n_ordercuttime]
      ,[n_orderdispatchedtime]
      ,[n_ordercashedtime]
      ,[n_statusflags]
      ,[n_occasion]
      ,[n_vatcost]
      ,[n_totaltime]
      ,[n_drivetime]
      ,[n_paytype]
      ,[n_EstimatedTotalTime]
      ,[n_relnum]
      ,[n_rectime]
      ,[n_timetaken]
      ,[tstamp]
      ,[n_ordercost]
      ,[n_orderpoints]
      ,[n_delcomm]
      ,[n_distance]
      ,[n_orderoriginallyplacedtime]
      ,[str_datacashid]
  FROM [dbo].[createorder] 
  where TheDate='20130705' AND nStoreId=1186
  order by thedate, nstoreid