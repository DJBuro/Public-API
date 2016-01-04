<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Executive.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=WebDashboard.Web.ResourceHelper.GetString("Executive")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="float:left;">
    <h2>
<%
        if (Model.ForDateTime.HasValue)
        {
%>
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Company"), "Index", "Executive", new { forDate = Model.ForDateTime.Value.ToString("yyyy-MM-dd") }, null)%> > 
            <%=WebDashboard.Web.ResourceHelper.GetString("Region")%> > 
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Stores"), "Store", "Executive", new { forDate = Model.ForDateTime.Value.ToString("yyyy-MM-dd") }, null)%>
<%
        }
        else
        {
%>
        <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Company"), "Index", "Executive")%> > <%=WebDashboard.Web.ResourceHelper.GetString("Region")%> > <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Stores"), "Store", "Executive")%>
<%
        }
%>
    </h2> 
</div>
<div style="float:right; padding:10px;">
    <%=WebDashboard.Web.ResourceHelper.GetString("ShowDataFor")%>
    <select style="margin-left:5px;" onchange="window.open('<%=Url.Action("", "Executive/Region") %>' + '?forDate=' + this.options[this.selectedIndex].value,'_top')">
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
<table width="100%" id="dataTable">
<%
    foreach (var regionDashboard in Model.ExecutiveRegionDashboard.RegionalList)
    {%>
    <tr>
        <td class="tbBg" colspan="12">
            <table>
                <tr>
                    <td style="padding-right:10px;">
                        <h3><%=regionDashboard.Region.Name %></h3>
                    </td>
                    <td>
                        <h3>
                            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("StoresReporting").Replace("{storecount}", regionDashboard.RegionStoreCount.ToString()), "RegionalStore/" + regionDashboard.RegionId + "/Name" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive")%> 
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
        <td align="center"><%= regionDashboard.Region.Sales.ToString("N2")%></td>
        <td align="center"><%= regionDashboard.Region.SalesLastWeek.ToString("N2")%></td>
        <td align="center"><%= regionDashboard.Region.SalesVsLastWeek%>%</td>
        <td align="center"><%= regionDashboard.Region.SalesLastYear.ToString("N2")%></td>
        <td align="center"><%= regionDashboard.Region.SalesVsLastYear%>%</td>
        <td align="center"><%= regionDashboard.Region.Orders%></td>
        <td align="center"><%= regionDashboard.Region.OrdersVsLastWeek%>%</td>
        <td align="center"><%= regionDashboard.Region.OrdersVsLastYear%>%</td>
        <td align="center"><%= regionDashboard.Region.Drivers%></td>
        <td align="center"><%= regionDashboard.Region.DriversDelivering%></td>
        <td align="center"></td>
        <td align="center"></td>
    </tr>
    <tr>
       <td>&nbsp;</td>
    </tr>
    <tr>
       <td colspan="12">
          <h3><%=WebDashboard.Web.ResourceHelper.GetString("CompResults").Replace("{compWithSalesCount}", regionDashboard.CompWithSalesCount.ToString()).Replace("{compCount}", regionDashboard.CompCount.ToString())%></h3>
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
        <td align="center"><%= regionDashboard.CompRegion.CompSales.ToString("N2")%></td>
        <td align="center"><%= regionDashboard.CompRegion.SalesLastYear.ToString("N2")%></td>
        <td align="center"><%= regionDashboard.CompRegion.CompSalesVsLastYear.ToString("N1")%>%</td>
        <td align="center"><%= regionDashboard.CompRegion.CompOrders%></td>
        <td align="center"><%= regionDashboard.CompRegion.OrdersLastYear%></td>
        <td align="center"><%= regionDashboard.CompRegion.CompOrdersVsLastYear.ToString("N1")%>%</td>
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
       <td colspan="16"><h3><%=WebDashboard.Web.ResourceHelper.GetString("StoreAverage")%></h3></td>
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
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("MinimumBrackets")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("MinimumBrackets")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("MinimumBrackets")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("MinimumBrackets")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Amount")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Amount")%></strong></td>
       <td align="center"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Amount")%></strong></td>
       <td align="center"><strong>(<%= Model.CurrencySymbol %>)</strong></td>
    </tr> 
    <tr>
        <td align="center"><%= regionDashboard.PerStoreAverage.Sales.ToString("N2")%></td>
        <td align="center"><%= regionDashboard.PerStoreAverage.Orders.ToString("N1")%></td>
        <td align="center"><%= regionDashboard.PerStoreAverage.LessThan30Min%>%</td>
        <td align="center"><%= regionDashboard.PerStoreAverage.LessThan45Min%>%</td>        
        <td align="center"><%= regionDashboard.PerStoreAverage.Make.ToString("N0")%></td>        
        <td align="center"><%= regionDashboard.PerStoreAverage.OTD.ToString("N0")%></td>
        <td align="center"><%= regionDashboard.PerStoreAverage.TTD.ToString("N0")%></td>
        <td align="center"><%= regionDashboard.PerStoreAverage.AverageDriveTime.ToString("N0")%></td>
        <td align="center"><%= regionDashboard.PerStoreAverage.Drivers.ToString("N2")%></td>
        <td align="center"><%= regionDashboard.PerStoreAverage.DriversDelivering.ToString("N1")%></td>
        <td align="center"><%= regionDashboard.PerStoreAverage.OPR.ToString("N2")%></td>
        <td align="center"><%= regionDashboard.PerStoreAverage.AverageSpend.ToString("N2")%></td>
    </tr>
    <tr>
       <td colspan="12">&nbsp;</td>
    </tr>
  <% 
    }//end foreach
%>

   
</table>
</div>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ValidatorContent" runat="server">
</asp:Content>