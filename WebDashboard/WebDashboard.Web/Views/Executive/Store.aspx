<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Executive.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=WebDashboard.Web.ResourceHelper.GetString("Executive")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="float:left;">
<%  
    int siteColumns = 18;
    
    if (Model.User.IsExecutiveDashboardGroupUser) 
    {
%>
        <h2>
<%
        if (Model.ForDateTime.HasValue)
        {
%>
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Company"), "Index", "Executive", new { forDate = Model.ForDateTime.Value.ToString("yyyy-MM-dd") }, null)%> > 
            <%=WebDashboard.Web.ResourceHelper.GetString("Stores")%>
<%
        }
        else
        {
%>
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Company"), "Index", "Executive")%> > 
            <%=WebDashboard.Web.ResourceHelper.GetString("Stores")%>
<%
        }
%>
        </h2>
<%
    }
    else
    {
%>
        <h2>
<%
        if (Model.ForDateTime.HasValue)
        {
%>
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Company"), "Index", "Executive", new { forDate = Model.ForDateTime.Value.ToString("yyyy-MM-dd") }, null)%> > 
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Region"), "Region", "Executive", new { forDate = Model.ForDateTime.Value.ToString("yyyy-MM-dd") }, null)%> > 
            <%=WebDashboard.Web.ResourceHelper.GetString("Stores")%>
<%
        }
        else
        {
%>
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Company"), "Index", "Executive")%> > 
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Region"), "Region", "Executive")%> > 
            <%=WebDashboard.Web.ResourceHelper.GetString("Stores")%>
<%
        }
%>    
        </h2>
<%
    }
%>
</div>
<div style="float:right; padding:10px;">
    <%=WebDashboard.Web.ResourceHelper.GetString("ShowDataFor")%>
<%
    if (Model.IsCompanySites)
    {
%>
    <select style="margin-left:5px;" onchange="window.open('<%=Url.Action("", "Executive/CompanyStore/" + Model.ExecutiveStoreDashboard.CompanyId.ToString()) %>' + '?forDate=' + this.options[this.selectedIndex].value,'_top')">    
<%
    }
    else
    {
%>
    <select style="margin-left:5px;" onchange="window.open('<%=Url.Action("", "Executive/Store/") %>' + '?forDate=' + this.options[this.selectedIndex].value,'_top')">    
<%
    }
%>  
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
        <td class="ColHeader1"><%=WebDashboard.Web.ResourceHelper.GetString("Store")%></td>
        <td class="ColHeaderOdd" colspan="3"><%=WebDashboard.Web.ResourceHelper.GetString("Sales")%></td>
        <td class="ColHeaderEven" colspan="3"><%=WebDashboard.Web.ResourceHelper.GetString("Orders")%></td>
        <td class="ColHeaderOdd" colspan="2"><%=WebDashboard.Web.ResourceHelper.GetString("DeliveryTime")%></td>
        <td class="ColHeaderEven" colspan="4"><%=WebDashboard.Web.ResourceHelper.GetString("Times")%></td>
        <td class="ColHeaderOdd" colspan="2"><%=WebDashboard.Web.ResourceHelper.GetString("Drivers")%></td>
        <td class="ColHeaderEven"></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Avg")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Store")%></td>
    </tr>
    <tr>
        <!-- Store -->
        <td class="ColHeader1"><%=WebDashboard.Web.ResourceHelper.GetString("Name")%></td>
        
        <!-- Sales -->
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Today")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("LW")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("LY")%></td>
        
        <!-- Orders -->
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Today")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("LW")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("LY")%></td>
        
        <!-- Delivery -->
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("LessThan30Min")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("LessThan45Min")%></td>
        
        <!-- Times -->
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Make")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("OTD")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("TTD")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Drive")%></td>   
        
        <!-- Drivers -->     
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Working")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Road")%></td>
        
        <!-- Misc -->
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("OPR")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Spend")%></td>
        
        <!-- Store -->
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Last")%></td>
    </tr>    
    <tr>
        <!-- Store -->
        <td class="ColHeader1"></td>
        
        <!-- Sales -->
        <td class="ColHeaderOdd">(<%= Model.CurrencySymbol %>)</td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        
        <!-- Orders -->
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Amount")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        
        <!-- Delivery -->
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Percentage")%></td>
        
        <!-- Times -->
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Minimum")%></td>  
        
        <!-- Drivers --> 
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Amount")%></td>
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Amount")%></td>
        
        <!-- Misc -->
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("Amount")%></td>
        <td class="ColHeaderEven">(<%= Model.CurrencySymbol %>)</td>
        
        <!-- Store -->
        <td class="ColHeaderOdd"><%=WebDashboard.Web.ResourceHelper.GetString("Updated")%></td>
    </tr>    
    <tr>
        <%  
            string action = "";
            if (Model.IsCompanySites) 
            {
                action = "CompanyStore/" + Model.ExecutiveStoreDashboard.CompanyId + "/";
            }
            else
            {
                action = "Store/";
            }
        %>
        
        <!-- Store -->
        <td class="ColHeader1">
            <a href="<%= Url.Action(action + "Name/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "Name/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Sales -->
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action(action + "Sales/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "Sales/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action(action + "SalesLW/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "SalesLW/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action(action + "SalesLY/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a> 
            <a href="<%= Url.Action(action + "SalesLY/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Orders -->
        <td class="ColHeaderEven">
            <a href="<%= Url.Action(action + "Orders/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "Orders/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action(action + "OrdersLW/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "OrdersLW/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action(action + "OrdersLY/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "OrdersLY/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Delivery -->
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action(action + "30min/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a> 
            <a href="<%= Url.Action(action + "30min/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action(action + "45min/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "45min/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Times -->
        <td class="ColHeaderEven">
            <a href="<%= Url.Action(action + "Make/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "Make/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action(action + "OTD/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "OTD/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action(action + "TTD/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "TTD/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action(action + "AverageDriveTime/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "AverageDriveTime/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Drivers --> 
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action(action + "Drivers/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "Drivers/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action(action + "Road/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "Road/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Misc -->
        <td class="ColHeaderEven">
            <a href="<%= Url.Action(action + "OPR/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "OPR/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action(action + "Avg/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action(action + "Avg/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Store -->
        <td class="ColHeaderOdd"></td>
    </tr>
    
    <!-- Divider line -->
    <tr class="sep">
       <td colspan="<%= siteColumns %>"></td>
    </tr>
    
    <%
    if (Model.ExecutiveStoreDashboard.StoreList != null)
    {
        foreach (var storeDashboard in Model.ExecutiveStoreDashboard.StoreList)
        {%>
         <tr>
            <!-- Store -->
            <td class="Col1"><%= storeDashboard.Name %></td>
            
            <!-- Sales -->
            <td class="ColOdd"><%= storeDashboard.Sales.ToString("N2")%></td>
            <td class="ColOdd"><%= Math.Round(storeDashboard.SalesVsLastWeek) %>%</td>
            <td class="ColOdd"><%= Math.Round(storeDashboard.SalesVsLastYear) %>%</td>
            
            <!-- Orders -->
            <td class="ColEven"><%= Math.Round(storeDashboard.Orders) %></td>
            <td class="ColEven"><%= Math.Round(storeDashboard.OrdersVsLastWeek) %>%</td>
            <td class="ColEven"><%= Math.Round(storeDashboard.OrdersVsLastYear) %>%</td>
            
            <!-- Delivery -->
            <td class="ColOdd"><%= Math.Round(storeDashboard.LessThan30Min) %>%</td>
            <td class="ColOdd"><%= Math.Round(storeDashboard.LessThan45Min) %>%</td>
            
            <!-- Times -->
            <td class="ColEven"><%= Math.Round(storeDashboard.Make) %></td>
            <td class="ColEven"><%= Math.Round(storeDashboard.OTD) %></td>
            <td class="ColEven"><%= Math.Round(storeDashboard.TTD) %></td>
            <td class="ColEven"><%= storeDashboard.AverageDriveTime %></td>
            
            <!-- Drivers --> 
            <td class="ColOdd"><%= Math.Round(storeDashboard.Drivers) %></td>
            <td class="ColOdd"><%= Math.Round(storeDashboard.DriversDelivering) %></td>
            
            <!-- Misc -->
            <td class="ColEven"><%= storeDashboard.OPR.ToString("N1")%></td>        
            <td class="ColEven"><%= storeDashboard.AverageSpend.ToString("N2")%></td>
<%            
                string lastUpdatedClass = "ColOdd";

                if (Model.ForDateTime == null)
                {
                    if (storeDashboard.LastUpdated.HasValue)
                    {
                        TimeSpan timeSpan = DateTime.Now - storeDashboard.LastUpdated.Value;

                        if (timeSpan.TotalMinutes > 10)
                        {
                            lastUpdatedClass = "ColOddError";
                        }
                    }
                    else
                    {
                        lastUpdatedClass = "ColOddError";
                    }
                }
%>
            
            <!-- Store -->
            <td class="<%= lastUpdatedClass %>">        
<% 
                if (!storeDashboard.LastUpdated.HasValue)
                {
%>
                    <%= "-" %>
<%
                }
                else if (Model.ForDateTime != null ||
                    storeDashboard.LastUpdated.Value.Day == DateTime.Now.Day && storeDashboard.LastUpdated.Value.Month == DateTime.Now.Month)
                {
%>
                <%= storeDashboard.LastUpdated.Value.ToString("HH:mm:ss") %>
<%  
                }
                else
                { 
%>
                <%= storeDashboard.LastUpdated.Value.ToString("dd/MM/yyyy HH:mm:ss") %>
<%  
                }
%>
            </td>
         </tr>
        
        <!-- Divider line -->
        <tr class="sep">
           <td colspan="<%= siteColumns %>"></td>       
        </tr>
    
      <% 
        }//end foreach
    }
%>
</table>
</div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ValidatorContent" runat="server">
</asp:Content>