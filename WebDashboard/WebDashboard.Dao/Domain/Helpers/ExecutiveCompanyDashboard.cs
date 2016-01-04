
using System.Collections.Generic;
using System;

namespace WebDashboard.Dao.Domain.Helpers
{
    public class ExecutiveGroupDashboard
    {
        // Averages
        public double SalesTodayValueAverage { get; set; }
        public double SalesLWValueAverage { get; set; }
        public double SalesLWPercentageAverage { get; set; }
        public double SalesLYValueAverage { get; set; }
        public double SalesLYPercentageAverage { get; set; }
        
        public double OrdersTodayCountAverage { get; set; }
        public double OrdersLWPercentageAverage { get; set; }
        public double OrdersLYPercentageAverage { get; set; }
        
        public double DriversWorkCountAverage { get; set; }
        public double DriversRoadcountAverage { get; set; }
        
        public double CompSalesTodayValueAverage { get; set; }
        public double CompSalesLYPercentageAverage { get; set; }
        public double CompSalesLYValueAverage { get; set; }
        
        public double CompOrdersTodayCountAverage { get; set; }
        public double CompOrdersLYCountAverage { get; set; }
        public double CompOrdersLYPercentageAverage { get; set; }
        
        public double DelTimeLT30MinAverage { get; set; }
        public double DelTimeLT45MinAverage { get; set; }
        
        public double MakeMinsAverage { get; set; }
        public double OTDMinsAverage { get; set; }
        public double TTDMinsAverage { get; set; }
        public double DriveMinsAverage { get; set; }
        public double OPRCountAverage { get; set; }
        public double AvgSpendValueAverage { get; set; }

        // Totals
        public double SalesTodayValueTotal { get; set; }
        public double SalesLWValueTotal { get; set; }
        public double SalesLWPercentageTotal { get; set; }
        public double SalesLYValueTotal { get; set; }
        public double SalesLYPercentageTotal { get; set; }

        public double OrdersTodayCountTotal { get; set; }
        public double OrdersLWPercentageTotal { get; set; }
        public double OrdersLYPercentageTotal { get; set; }

        public double DriversWorkCountTotal { get; set; }
        public double DriversRoadCountTotal { get; set; }

        public double CompSalesTodayValueTotal { get; set; }
        public double CompSalesLYValueTotal { get; set; }
        public double CompSalesLYPercentageTotal { get; set; }

        public double CompOrdersTodayCountTotal { get; set; }
        public int CompOrdersLYCountTotal { get; set; }
        public double CompOrdersLYPercentageTotal { get; set; }

        public double DelTimeLT30MinTotal { get; set; }
        public double DelTimeLT45MinTotal { get; set; }

        public double MakeMinsTotal { get; set; }
        public double OTDMinsTotal { get; set; }
        public double TTDMinsTotal { get; set; }
        public double DriveMinsTotal { get; set; }
        public double OPRCountTotal { get; set; }
        public double AvgSpendValueTotal { get; set; }
    
        public List<ExecutiveCompanyDashboard> CompanyDashboards { get; set; }

        public int StoreCountAverage { get; set; }
        public int UploadingStoreCountAverage { get; set; }
        
        public int StoreCountTotal { get; set; }
        public int UploadingStoreCountTotal { get; set; }
        
        public ExecutiveGroupDashboard()
        {
            this.CompanyDashboards = new List<ExecutiveCompanyDashboard>();
        }
    }
    
    public class ExecutiveCompanyDashboard : IComparable
    {
        //todo:
        //Site	Sales	Sales Comp%	vLW%	Orders	OrdComp%	%<30	%>45	Make	OTD	TTD	Drive	OPR	Drivers On	Avg Spend
        //Company Total
        //Company PSA
        public ExecutiveData Company { get; set; }
        public ExecutiveData CompCompany { get; set; }
        public ExecutiveData PerStoreAverage { get; set; }
        public int StoreCount { get; set; }
        public int UploadingStoreCount { get; set; }
        public int CompWithSalesCount { get; set; }
        public int CompCount { get; set; }

        public ExecutiveCompanyDashboard()
        {
            Company = new ExecutiveData();
            CompCompany = new ExecutiveData();
            PerStoreAverage = new ExecutiveData();
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            ExecutiveCompanyDashboard executiveCompanyDashboard = (ExecutiveCompanyDashboard)obj;
            return executiveCompanyDashboard.Company.Name.CompareTo(this.Company.Name) * -1;
        }

        #endregion
    }

    public class ExecutiveRegionDashboard
    {
        //todo:
        //Site	Sales	Sales Comp%	vLW%	Orders	OrdComp%	%<30	%>45	Make	OTD	TTD	Drive	OPR	Drivers On	Avg Spend
        //Region Total
        //Region PSA
        public ExecutiveData Region { get; set; }
        public ExecutiveData CompRegion { get; set; }
        public ExecutiveData CompCompany { get; set; }
        public ExecutiveData PerStoreAverage { get; set; }
        public int RegionStoreCount { get; set; }
        public int RegionId { get; set; }
        public IList<ExecutiveRegionDashboard> RegionalList { get; set; }
        public int CompWithSalesCount { get; set; }
        public int CompCount { get; set; }

        public ExecutiveRegionDashboard()
        {
            Region = new ExecutiveData();
            CompRegion = new ExecutiveData();
            CompCompany = new ExecutiveData();
            PerStoreAverage = new ExecutiveData();
        }
    }

    public class ExecutiveStoreDashboard
    {
        //todo:
        //Site	Sales	Sales Comp%	vLW%	Orders	OrdComp%	%<30	%>45	Make	OTD	TTD	Drive	OPR	Drivers On	Avg Spend
        //Region Total
        //Region PSA
        public int? CompanyId { get; set; }
        public ExecutiveData Store { get; set; }
        public IList<ExecutiveData> StoreList { get; set; } 
        public ExecutiveStoreDashboard()
        {
            Store = new ExecutiveData();
        }
    }

    public interface IExecutiveData
    {
        string Name { get; set; }
        double Sales { get; set; } //£
        //double SalesFlash { get; set; }
        double SalesLastWeek { get; set; }
        double SalesLastYear { get; set; }

        double SalesVsLastWeek { get; set; }
        double SalesVsLastYear { get; set; } //%

        double Orders { get; set; } //count
        int OrdersLastWeek { get; set; } 
        int OrdersLastYear { get; set; }

        double OrdersVsLastWeek { get; set; } //%
        double OrdersVsLastYear { get; set; } //%

        double LessThan30Min { get; set; }
        double LessThan45Min { get; set; }  
 
        double Make { get; set; }   
        double OTD { get; set; }
        double TTD { get; set; }

        double Drivers { get; set; }
        double DriversDelivering { get; set; }
        double OPR { get; set; }
        double AverageDriveTime { get; set; }

        double AverageSpend { get; set; }
    }

    public class ExecutiveData : IExecutiveData
    {
        #region IExecutiveData Members

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public double Sales { get; set;}
        public double CompSales { get; set; }
        public double SalesLastWeek { get; set; }
        public double SalesLastYear { get; set; }
        public double SalesVsLastWeek { get; set; }
        public double SalesVsLastYear { get; set; }
        public double CompSalesVsLastYear { get; set; }
        public double Orders { get; set; }
        public double CompOrders { get; set; }
        public int OrdersLastWeek { get; set; }
        public int OrdersLastYear { get; set; }
        public double OrdersVsLastWeek { get; set; }
        public double OrdersVsLastYear { get; set; }
        public double CompOrdersVsLastYear { get; set; }
        public double LessThan30Min { get; set; }
        public double LessThan45Min { get; set; }
        public double Make { get; set; }
        public double OTD { get; set; }
        public double TTD { get; set; }
        public double Drivers { get; set; }
        public double DriversDelivering { get; set; }
        public double OPR { get; set; }
        public double AverageDriveTime { get; set; }
        public double AverageSpend { get; set; }
        public DateTime? LastUpdated { get; set; }

        #endregion
    }

    public class CompExecutiveData : IExecutiveData
    {
        #region IExecutiveData Members

        public string Name { get; set; }
        public double Sales { get; set; }
        public double SalesLastWeek { get; set; }
        public double SalesLastYear { get; set; }
        public double SalesVsLastWeek { get; set; }
        public double SalesVsLastYear { get; set; }
        public double Orders { get; set; }
        public int OrdersLastWeek { get; set; }
        public int OrdersLastYear { get; set; }
        public double OrdersVsLastWeek { get; set; }
        public double OrdersVsLastYear { get; set; }
        public double LessThan30Min { get; set; }
        public double LessThan45Min { get; set; }
        public double Make { get; set; }
        public double OTD { get; set; }
        public double TTD { get; set; }
        public double Drivers { get; set; }
        public double DriversDelivering { get; set; }
        public double OPR { get; set; }
        public double AverageDriveTime { get; set; }
        public double AverageSpend { get; set; }

        #endregion

    }

}
