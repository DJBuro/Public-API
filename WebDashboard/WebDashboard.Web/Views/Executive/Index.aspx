<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Executive.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=WebDashboard.Web.ResourceHelper.GetString("Executive")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%  
    if (Model.User.IsExecutiveDashboardGroupUser) 
    {
        int companyColumns = 26;
%>
<div style="float:left;">
    <h2>
<%
        if (Model.ForDateTime.HasValue)
        {
%>
            <%=WebDashboard.Web.ResourceHelper.GetString("Company")%> > 
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Stores"), "Store", "Executive", new { forDate = Model.ForDateTime.Value.ToString("yyyy-MM-dd") }, null)%>
<%
        }
        else
        {
%>
            <%=WebDashboard.Web.ResourceHelper.GetString("Company")%> > 
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Stores"), "Store","Executive") %>
<%
        }
%>
    </h2> 
</div>
<div style="float:right; padding:10px;">
    <%=WebDashboard.Web.ResourceHelper.GetString("ShowDataFor")%>
    <select style="margin-left:5px;" onchange="window.open('<%=Url.Action("", "Executive") %>' + '?forDate=' + this.options[this.selectedIndex].value,'_top')">
        <option value="Today"><%=WebDashboard.Web.ResourceHelper.GetString("TodayFull")%></option>
<% 
    foreach (KeyValuePair<string, bool> availableTime in Model.AvailableDates)
    {
        if (availableTime.Value)
        {
%> 
        <option selected='selected' value="<%= availableTime.Key%>"><%= availableTime.Key%></option>
<%
        }
        else
        {
%> 
        <option value="<%= availableTime.Key%>"><%= availableTime.Key%></option>
<%
        }
    }
%>
    </select>
</div>

<div style="clear:both;">

<table width="100%" id="dataTable2">
    <tr class="HeaderRow">
        <td class="ColHeader1"><%=WebDashboard.Web.ResourceHelper.GetString("Country")%></td>
        <td class="ColHeaderOdd" colspan="5"><%=WebDashboard.Web.ResourceHelper.GetString("Sales")%></td>
        <td class="ColHeaderEven" colspan="3"><%=WebDashboard.Web.ResourceHelper.GetString("Orders")%></td>
        <td class="ColHeaderOdd" colspan="2"><%=WebDashboard.Web.ResourceHelper.GetString("Drivers")%></td>
        <td class="ColHeaderEven" colspan="3"><%=WebDashboard.Web.ResourceHelper.GetString("CompSales").Replace(" ", "&nbsp;")%></td>
        <td class="ColHeaderOdd" colspan="3"><%=WebDashboard.Web.ResourceHelper.GetString("CompOrders").Replace(" ", "&nbsp;")%></td>
        <td class="ColHeaderEven" colspan="6"><%=WebDashboard.Web.ResourceHelper.GetString("Times")%></td>
        <td class="ColHeaderOdd"></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Avg")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Stores")%></td>  
    </tr>
    <tr>
        <!-- Company -->
        <td class="ColHeader1"></td>
        
        <!-- Sales -->
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Today")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("LW")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("LW")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("LY")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("LY")%></td>
        
        <!-- Orders -->
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Today")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("LW")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("LY")%></td>
        
        <!-- Drivers -->
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Work")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Road")%></td>
        
        <!-- Comp Sales -->
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Today")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("LY")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("LY")%></td>
        
        <!-- Comp Orders -->
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Today")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("LY")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("LY")%></td>
        
        <!-- Store Average -->
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("LessThan30")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("LessThan30")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Make")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("OTD")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("TTD")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Drive")%></td>  
              
        <!-- Misc -->
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("OPR")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Spend")%></td>
        
        <!-- Stores Reporting -->
        <td class="ColHeaderEven"></td>
    </tr>    
    <tr>
        <!-- Company -->
        <td class="ColHeader1"></td>
        
        <!-- Sales -->
        <td class="ColHeaderOdd">(<%=Model.CurrencySymbol %>)</td>
        <td class="ColHeaderOdd">(<%=Model.CurrencySymbol %>)</td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        <td class="ColHeaderOdd">(<%=Model.CurrencySymbol %>)</td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        
        <!-- Orders -->
        <td class="ColHeaderEven">(<%=WebDashboard.Web.ResourceHelper.GetString("Amount")%>)</td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        
        <!-- Drivers -->
        <td class="ColHeaderOdd">(<%=WebDashboard.Web.ResourceHelper.GetString("Amount")%>)</td>
        <td class="ColHeaderOdd">(<%=WebDashboard.Web.ResourceHelper.GetString("Amount")%>)</td>
        
        <!-- Comp Sales -->
        <td class="ColHeaderEven">(<%=Model.CurrencySymbol %>)</td>
        <td class="ColHeaderEven">(<%=Model.CurrencySymbol %>)</td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        
        <!-- Comp Orders -->
        <td class="ColHeaderOdd">(<%=WebDashboard.Web.ResourceHelper.GetString("Amount")%>)</td>
        <td class="ColHeaderOdd">(<%=WebDashboard.Web.ResourceHelper.GetString("Amount")%>)</td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        
        <!-- Store Average -->     
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Sales")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%></td>
        <td class="ColHeaderEven">(<%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%>)</td>
        <td class="ColHeaderEven">(<%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%>)</td>
        <td class="ColHeaderEven">(<%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%>)</td>
        <td class="ColHeaderEven">(<%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%>)</td>  
              
        <!-- Misc -->
        <td class="ColHeaderOdd">(<%=WebDashboard.Web.ResourceHelper.GetString("Amount")%>)</td>
        <td class="ColHeaderOdd">(<%= Model.CurrencySymbol %>)</td>
        
        <!-- Stores Reporting -->
        <td class="ColHeaderEven"></td>
    </tr>    

    <!-- Spacer row -->
    <tr style="height:3px;">
        <!-- Company -->
        <td class="ColHeader1"></td>
        
        <!-- Sales -->
        <td class="ColHeaderOdd"></td>
        <td class="ColHeaderOdd"></td>
        <td class="ColHeaderOdd"></td>
        <td class="ColHeaderOdd"></td>
        <td class="ColHeaderOdd"></td>
        
        <!-- Orders -->
        <td class="ColHeaderEven"></td>
        <td class="ColHeaderEven"></td>
        <td class="ColHeaderEven"></td>
        
        <!-- Drivers -->
        <td class="ColHeaderOdd"></td>
        <td class="ColHeaderOdd"></td>
        
        <!-- Comp Sales -->
        <td class="ColHeaderEven"></td>
        <td class="ColHeaderEven"></td>
        <td class="ColHeaderEven"></td>
        
        <!-- Comp Orders -->
        <td class="ColHeaderOdd"></td>
        <td class="ColHeaderOdd"></td>
        <td class="ColHeaderOdd"></td>
        
        <!-- Store Average -->     
        <td class="ColHeaderEven"></td>
        <td class="ColHeaderEven"></td>
        <td class="ColHeaderEven"></td>
        <td class="ColHeaderEven"></td>
        <td class="ColHeaderEven"></td>
        <td class="ColHeaderEven"></td>  
              
        <!-- Misc -->
        <td class="ColHeaderOdd"></td>
        <td class="ColHeaderOdd"></td>
        
        <!-- Stores Reporting -->
        <td class="ColHeaderEven"></td>
    </tr>
    
    <!-- Divider line -->
    <tr class="sep">
       <td colspan="<%= companyColumns %>"></td>
    </tr>
<%    
    foreach (WebDashboard.Dao.Domain.Helpers.ExecutiveCompanyDashboard executiveCompanyDashboard in Model.ExecutiveGroupDashboard.CompanyDashboards)
    {
%>
    <tr>  
        <td class="Col1">
<%
        if (Model.ForDateTime.HasValue)
        {
%>
            <%= Html.ActionLink(executiveCompanyDashboard.Company.Name.Replace("Papa Johns ", ""), "CompanyStore/" + executiveCompanyDashboard.Company.CompanyId, "Executive", new { forDate = Model.ForDateTime.Value.ToString("yyyy-MM-dd") }, null)%>
<%
        }
        else
        {
%>
            <%= Html.ActionLink(executiveCompanyDashboard.Company.Name.Replace("Papa Johns ", ""), "CompanyStore/" + executiveCompanyDashboard.Company.CompanyId, "Executive")%>
<%
        }
%>
        
        </td>
        
        <!-- Sales -->
        <td class="ColOdd"><%= Math.Round(executiveCompanyDashboard.Company.Sales).ToString("N0")%></td>
        <td class="ColOdd"><%= Math.Round(executiveCompanyDashboard.Company.SalesLastWeek).ToString("N0")%></td>
        <td class="ColOdd"><%= Math.Round(executiveCompanyDashboard.Company.SalesVsLastWeek).ToString("N0")%></td>
        <td class="ColOdd"><%= Math.Round(executiveCompanyDashboard.Company.SalesLastYear).ToString("N0")%></td>
        <td class="ColOdd"><%= Math.Round(executiveCompanyDashboard.Company.SalesVsLastYear).ToString("N0")%></td>
        
        <!-- Orders -->
        <td class="ColEven"><%= executiveCompanyDashboard.Company.Orders.ToString("N0")%></td>
        <td class="ColEven"><%= Math.Round(executiveCompanyDashboard.Company.OrdersVsLastWeek).ToString("N0")%></td>
        <td class="ColEven"><%= Math.Round(executiveCompanyDashboard.Company.OrdersVsLastYear).ToString("N0")%></td>
        
        <!-- Drivers -->        
        <td class="ColOdd"><%= executiveCompanyDashboard.Company.Drivers.ToString("N0")%></td>
        <td class="ColOdd"><%= executiveCompanyDashboard.Company.DriversDelivering.ToString("N0")%></td>
        
        <!-- Comp Sales -->
        <td class="ColEven"><%= Math.Round(executiveCompanyDashboard.CompCompany.Sales).ToString("N0")%></td>
        <td class="ColEven"><%= Math.Round(executiveCompanyDashboard.CompCompany.SalesLastYear).ToString("N0")%></td>
        <td class="ColEven"><%= Math.Round(executiveCompanyDashboard.CompCompany.SalesVsLastYear).ToString("N0")%></td>
        
        <!-- Comp Orders -->
        <td class="ColOdd"><%= executiveCompanyDashboard.CompCompany.Orders.ToString("N0")%></td>
        <td class="ColOdd"><%= executiveCompanyDashboard.CompCompany.OrdersLastYear.ToString("N0")%></td>
        <td class="ColOdd"><%= Math.Round(executiveCompanyDashboard.CompCompany.OrdersVsLastYear).ToString("N0")%></td>
        
        <!-- Store Average -->
        <td class="ColEven"><%= Math.Round(executiveCompanyDashboard.PerStoreAverage.LessThan30Min).ToString("N0")%></td>
        <td class="ColEven"><%= Math.Round(executiveCompanyDashboard.PerStoreAverage.LessThan45Min).ToString("N0")%></td>        
        <td class="ColEven"><%= Math.Round(executiveCompanyDashboard.PerStoreAverage.Make).ToString("N0")%></td>        
        <td class="ColEven"><%= Math.Round(executiveCompanyDashboard.PerStoreAverage.OTD).ToString("N0")%></td>
        <td class="ColEven"><%= Math.Round(executiveCompanyDashboard.PerStoreAverage.TTD).ToString("N0")%></td>
        <td class="ColEven"><%= executiveCompanyDashboard.PerStoreAverage.AverageDriveTime.ToString("N0")%></td>
        
        <!-- Misc -->
        <td class="ColOdd"><%= executiveCompanyDashboard.PerStoreAverage.OPR.ToString("N2")%></td>
        <td class="ColOdd"><%= executiveCompanyDashboard.PerStoreAverage.AverageSpend.ToString("N2")%></td>
        
        <!-- Stores Reporting -->
        <td class="ColEven"><%= executiveCompanyDashboard.UploadingStoreCount%>/<%= executiveCompanyDashboard.StoreCount%></td>
    </tr>
<% 

    }
%>
    
    <!-- Divider line -->
    <tr class="sep">
       <td colspan="<%= companyColumns %>"></td>       
    </tr>
    
    <!-- Averages -->
    <tr>
        <td class="ColFoot1">&nbsp;<%=WebDashboard.Web.ResourceHelper.GetString("Average")%></td>
        
        <!-- Sales -->
        <td class="ColFootOdd"><%= Math.Round(Model.ExecutiveGroupDashboard.SalesTodayValueAverage).ToString("N0")%></td>
        <td class="ColFootOdd"><%= Math.Round(Model.ExecutiveGroupDashboard.SalesLWValueAverage).ToString("N0")%></td>
        <td class="ColFootOdd">-</td>
        <td class="ColFootOdd">-</td>
        <td class="ColFootOdd">-</td>
        
        <!-- Orders -->
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.OrdersTodayCountAverage).ToString("N0")%></td>
        <td class="ColFootEven">-</td>
        <td class="ColFootEven">-</td>
        
        <!-- Drivers -->        
        <td class="ColFootOdd"><%= Model.ExecutiveGroupDashboard.DriversWorkCountAverage.ToString("N1")%></td>
        <td class="ColFootOdd"><%= Model.ExecutiveGroupDashboard.DriversRoadcountAverage.ToString("N1")%></td>
        
        <!-- Comp Sales -->
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.CompSalesTodayValueAverage).ToString("N0")%></td>
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.CompSalesLYValueAverage).ToString("N0")%></td>
        <td class="ColFootEven">-</td>
        
        <!-- Comp Orders -->
        <td class="ColFootOdd"><%= Math.Round(Model.ExecutiveGroupDashboard.CompOrdersTodayCountAverage).ToString("N0")%></td>
        <td class="ColFootOdd"><%= Math.Round(Model.ExecutiveGroupDashboard.CompOrdersLYCountAverage).ToString("N0")%></td>
        <td class="ColFootOdd">-</td>

        <!-- Store Average -->
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.DelTimeLT30MinAverage).ToString("N0")%></td>
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.DelTimeLT45MinAverage).ToString("N0")%></td>        
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.MakeMinsAverage).ToString("N0")%></td>        
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.OTDMinsAverage).ToString("N0")%></td>
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.TTDMinsAverage).ToString("N0")%></td>
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.DriveMinsAverage).ToString("N0")%></td>
        
        <!-- Misc -->
        <td class="ColFootOdd"><%= Model.ExecutiveGroupDashboard.OPRCountAverage.ToString("N2")%></td>
        <td class="ColFootOdd"><%= Model.ExecutiveGroupDashboard.AvgSpendValueAverage.ToString("N2")%></td>
        
        <!-- Stores Reporting -->
        <td class="ColFootEven">-</td>
    </tr>
    
    <!-- Totals -->
    <tr>
        <td class="ColFoot1">&nbsp;<%=WebDashboard.Web.ResourceHelper.GetString("Total")%></td>
        
        <!-- Sales -->
        <td class="ColFootOdd"><%= Math.Round(Model.ExecutiveGroupDashboard.SalesTodayValueTotal).ToString("N0")%></td>
        <td class="ColFootOdd"><%= Math.Round(Model.ExecutiveGroupDashboard.SalesLWValueTotal).ToString("N0")%></td>
        <td class="ColFootOdd"><%= Math.Round(Model.ExecutiveGroupDashboard.SalesLWPercentageTotal).ToString("N0")%></td>
        <td class="ColFootOdd"><%= Math.Round(Model.ExecutiveGroupDashboard.SalesLYValueTotal).ToString("N0")%></td>
        <td class="ColFootOdd"><%= Math.Round(Model.ExecutiveGroupDashboard.SalesLYPercentageTotal).ToString("N0")%></td>
        
        <!-- Orders -->
        <td class="ColFootEven"><%= Model.ExecutiveGroupDashboard.OrdersTodayCountTotal.ToString("N0")%></td>
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.OrdersLWPercentageTotal).ToString("N0")%></td>
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.OrdersLYPercentageTotal).ToString("N0")%></td>
        
        <!-- Drivers -->        
        <td class="ColFootOdd"><%= Model.ExecutiveGroupDashboard.DriversWorkCountTotal.ToString("N0")%></td>
        <td class="ColFootOdd"><%= Model.ExecutiveGroupDashboard.DriversRoadCountTotal.ToString("N0")%></td>
        
        <!-- Comp Results Sales -->
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.CompSalesTodayValueTotal).ToString("N0")%></td>
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.CompSalesLYValueTotal).ToString("N0")%></td>
        <td class="ColFootEven"><%= Math.Round(Model.ExecutiveGroupDashboard.CompSalesLYPercentageTotal).ToString("N0")%></td>
        
        <!-- Comp Results Orders -->
        <td class="ColFootOdd"><%= Model.ExecutiveGroupDashboard.CompOrdersTodayCountTotal.ToString("N0")%></td>
        <td class="ColFootOdd"><%= Model.ExecutiveGroupDashboard.CompOrdersLYCountTotal.ToString("N0")%></td>
        <td class="ColFootOdd"><%= Math.Round(Model.ExecutiveGroupDashboard.CompOrdersLYPercentageTotal).ToString("N0")%></td>
        
        <!-- Store Average -->
        <td class="ColFootEven">-</td>
        <td class="ColFootEven">-</td>        
        <td class="ColFootEven">-</td>        
        <td class="ColFootEven">-</td>
        <td class="ColFootEven">-</td>
        <td class="ColFootEven">-</td>
        
        <!-- Misc -->
        <td class="ColFootOdd">-</td>
        <td class="ColFootOdd">-</td>
        
        <!-- Stores Reporting -->
        <td class="ColFootEven"><%= Model.ExecutiveGroupDashboard.UploadingStoreCountTotal%>/<%= Model.ExecutiveGroupDashboard.StoreCountTotal%></td>
    </tr>
    
</table>
</div>
<%
    }
    else
    {
%>
<div style="float:left;">
    <h2>
<%
        if (Model.ForDateTime.HasValue)
        {
%>
            <%=WebDashboard.Web.ResourceHelper.GetString("Company")%> > 
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Region"), "Region", "Executive", new { forDate = Model.ForDateTime.Value.ToString("yyyy-MM-dd") }, null)%> > 
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Store"), "Store", "Executive", new { forDate = Model.ForDateTime.Value.ToString("yyyy-MM-dd") }, null)%>
<%
        }
        else
        {
%>
            <%=WebDashboard.Web.ResourceHelper.GetString("Company")%> > 
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Region"), "Region", "Executive")%> > 
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Store"), "Store", "Executive")%>
<%
        }
%>
    </h2> 
</div>
<div style="float:right; padding:10px;">
    <%=WebDashboard.Web.ResourceHelper.GetString("ShowDataFor")%>
    <select style="margin-left:5px;" onchange="window.open('<%=Url.Action("", "Executive") %>' + '?forDate=' + this.options[this.selectedIndex].value,'_top')">
        <option value="Today"><%=WebDashboard.Web.ResourceHelper.GetString("TodayFull")%></option>
<% 
    foreach (KeyValuePair<string, bool> availableTime in Model.AvailableDates)
    {
        if (availableTime.Value)
        {
%> 
        <option selected='selected' value="<%= availableTime.Key%>"><%= availableTime.Key%></option>
<%
        }
        else
        {
%> 
        <option value="<%= availableTime.Key%>"><%= availableTime.Key%></option>
<%
        }
    }
%>
    </select>
</div>

<div style="clear:both;">

<table id="dataTable" width="100%">
    <tr>
        <td class="tbBg" colspan="12">
            <table>
                <tr>
                    <td style="padding-right:10px;">
                        <h3><%=WebDashboard.Web.ResourceHelper.GetString("Company")%></h3>
                    </td>
                    <td>
                        <h3>
<%
    if (Model.ForDateTime.HasValue)
    {
%>
                            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("StoresReporting").Replace("{storecount}", Model.ExecutiveCompanyDashboard.StoreCount.ToString()), "Store", "Executive", new { forDate = Model.ForDateTime.Value.ToString("yyyy-MM-dd") }, null)%>
<%
    }
    else
    {
%>
                            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("StoresReporting").Replace("{storecount}", Model.ExecutiveCompanyDashboard.StoreCount.ToString()), "Store", "Executive")%>
<%
    }
%>
                        </h3>
                   </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
       <td align="center" colspan="5"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Sales")%></strong></td>
       <td align="center" colspan="3"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Orders")%></strong></td>
       <td align="center" colspan="2"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Drivers")%></strong></td>    
       <td align="center"><strong></strong></td>
       <td align="center"><strong></strong></td>          
    </tr>   
    <tr>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("TodayCurrency").Replace("{currency}", Model.CurrencySymbol) %></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("LWCurrency").Replace("{currency}", Model.CurrencySymbol) %></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("LWPercent")%></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("LYCurrency").Replace("{currency}", Model.CurrencySymbol) %></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("LYPercent")%></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("TodayCount")%></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("LWPercent")%></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("LYPercent")%></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("WorkingCount")%></strong></td>    
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("RoadCount")%></strong></td>
       <td align="center"><strong></strong></td>
       <td align="center"><strong></strong></td>
    </tr>   
    <tr>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.Company.Sales.ToString("N2")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.Company.SalesLastWeek.ToString("N2")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.Company.SalesVsLastWeek%>%</td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.Company.SalesLastYear.ToString("N2")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.Company.SalesVsLastYear%>%</td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.Company.Orders%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.Company.OrdersVsLastWeek%>%</td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.Company.OrdersVsLastYear%>%</td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.Company.Drivers%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.Company.DriversDelivering%></td>
        <td align="center"></td>
        <td align="center"></td>   
    </tr>
    <tr>
       <td>&nbsp;</td>
    </tr>
    <tr>
       <td colspan="12">
          <h3><%=WebDashboard.Web.ResourceHelper.GetString("CompResults").Replace("{compWithSalesCount}", Model.ExecutiveCompanyDashboard.CompWithSalesCount.ToString()).Replace("{compCount}", Model.ExecutiveCompanyDashboard.CompCount.ToString())%></h3>
       </td>
    </tr>
    <tr>
       <td align="center" colspan="3"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Sales")%></strong></td>
       <td align="center" colspan="3"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Orders")%></strong></td>
       <td align="center"><strong></strong></td>
       <td align="center"><strong></strong></td>
       <td align="center"><strong></strong></td>
       <td align="center"><strong></strong></td>
       <td align="center"><strong></strong></td>
       <td align="center"><strong></strong></td>
    </tr>   
    <tr>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("TodayCurrency").Replace("{currency}", Model.CurrencySymbol) %></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("LYCurrency").Replace("{currency}", Model.CurrencySymbol) %></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("LYPercent")%></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("TodayCount")%></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("LYCount")%></strong></td>
       <td align="center"><strong><%= WebDashboard.Web.ResourceHelper.GetString("LYPercent")%></strong></td>
       <td align="center"><strong></strong></td>    
       <td align="center"><strong></strong></td>
       <td align="center"><strong></strong></td>
       <td align="center"><strong></strong></td>          
       <td align="center"><strong></strong></td>
       <td align="center"><strong></strong></td>
    </tr>   
    <tr>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.CompCompany.CompSales.ToString("N2")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.CompCompany.SalesLastYear.ToString("N2")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.CompCompany.CompSalesVsLastYear.ToString("N1")%>%</td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.CompCompany.CompOrders%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.CompCompany.OrdersLastYear%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.CompCompany.CompOrdersVsLastYear.ToString("N1")%>%</td>
        <td align="center"></td>
        <td align="center"></td> 
        <td align="center"></td>
        <td align="center"></td> 
        <td align="center"></td>
        <td align="center"></td>   
    </tr>
    <tr>
       <td>&nbsp;</td>
    </tr>
    <tr>
       <td colspan="12"><h3><%=WebDashboard.Web.ResourceHelper.GetString("StoreAverage")%></h3></td>
    </tr>
    <tr>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Sales")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Orders")%></strong></td>
       <td align="center" colspan="2"><strong><%=WebDashboard.Web.ResourceHelper.GetString("DeliveryTime")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Make")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("OTD")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("TTD")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Drive")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Drivers")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("OnRoad")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("OPR")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("AvgSpend")%></strong></td>
    </tr> 
    <tr>
       <td align="center"><strong>(<%= Model.CurrencySymbol %>)</strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Amount")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("LessThan30Min")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("LessThan45Min")%></strong></td>
       <td align="center"><strong>(<%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%>)</strong></td>
       <td align="center"><strong>(<%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%>)</strong></td>
       <td align="center"><strong>(<%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%>)</strong></td>
       <td align="center"><strong>(<%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%>)</strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Amount")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Amount")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Amount")%></strong></td>
       <td align="center"><strong>(<%= Model.CurrencySymbol %>)</strong></td>
    </tr> 
    <tr>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.Sales.ToString("N2")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.Orders.ToString("N1")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.LessThan30Min.ToString("N0")%>%</td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.LessThan45Min.ToString("N0")%>%</td>        
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.Make.ToString("N0")%></td>        
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.OTD.ToString("N0")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.TTD.ToString("N0")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.AverageDriveTime.ToString("N0")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.Drivers.ToString("N2")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.DriversDelivering.ToString("N1")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.OPR.ToString("N2")%></td>
        <td align="center"><%= Model.ExecutiveCompanyDashboard.PerStoreAverage.AverageSpend.ToString("N2")%></td>
    </tr>
    <tr>
       <td colspan="12">&nbsp;</td>
    </tr>
</table>
</div>
<%
    }
%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ValidatorContent" runat="server">
</asp:Content>
