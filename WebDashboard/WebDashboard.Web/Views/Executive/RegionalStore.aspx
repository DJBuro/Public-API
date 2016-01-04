<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Executive.Master"  Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
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
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Region"), "Region", "Executive", new { forDate = Model.ForDateTime.Value.ToString("yyyy-MM-dd") }, null)%> > 
            <%=Model.Region.Name %>
<%
        }
        else
        {
%>
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Company"), "Index", "Executive")%> > 
            <%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("Region"), "Region", "Executive")%> > 
            <%=Model.Region.Name %>
<%
        }
%>    
    </h2>
</div>
<div style="float:right; padding:10px;">
    <%=WebDashboard.Web.ResourceHelper.GetString("ShowDataFor")%>
    <select style="margin-left:5px;" onchange="window.open('<%=Url.Action("", "Executive/RegionalStore") %>/<%=Model.Region.Id%>?forDate=' + this.options[this.selectedIndex].value,'_top')">
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
    
<%  
    int siteColumns = 18;
%>

<table width="100%" id="dataTable2">
    <tr class="HeaderRow">
        <td class="ColHeader1"><%=WebDashboard.Web.ResourceHelper.GetString("Store")%></td>
        <td class="ColHeaderOdd" colspan="3"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Sales")%></strong></td>
        <td class="ColHeaderEven" colspan="3"><strong><%=WebDashboard.Web.ResourceHelper.GetString("Orders")%></strong></td>
        <td class="ColHeaderOdd" colspan="2"><strong><%=WebDashboard.Web.ResourceHelper.GetString("DeliveryTime")%></strong></td>
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
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("MinimumBrackets")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("MinimumBrackets")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("MinimumBrackets")%></td>
        <td class="ColHeaderEven"><%=WebDashboard.Web.ResourceHelper.GetString("MinimumBrackets")%></td>  
        
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
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/Name/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/Name/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>

        <!-- Sales -->
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/Sales/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/Sales/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/SalesLW/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/SalesLW/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/SalesLY/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a> 
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/SalesLY/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Orders -->
        <td class="ColHeaderEven">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/Orders/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/Orders/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/OrdersLW/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/OrdersLW/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/OrdersLY/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/OrdersLY/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Delivery -->
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/30min/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a> 
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/30min/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
            </td>
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/45min/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/45min/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
            </td>
        
        <!-- Times -->
        <td class="ColHeaderEven">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/Make/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/Make/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/OTD/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/OTD/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/TTD/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/TTD/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/AverageDriveTime/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/AverageDriveTime/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Drivers -->
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/Drivers/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/Drivers/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderOdd">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/Road/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/Road/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Misc -->
        <td class="ColHeaderEven">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/OPR/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/OPR/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        <td class="ColHeaderEven">
            <a href="<%= Url.Action("RegionalStore/" + Model.Region.Id + "/Avg/" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoUp.gif" alt="ascending" /></a>
            <a href="<%= Url.Action("RegionalStore/"+ Model.Region.Id +"/Avg/Desc" + (Model.ForDateTime.HasValue ? "?forDate=" + Model.ForDateTime.Value.ToString("yyyy-MM-dd") : ""), "Executive") %>"><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/icoDown.gif" alt="decending" /></a>
        </td>
        
        <!-- Store -->
        <td class="ColHeaderOdd"></td>
    </tr>
    
    <!-- Divider line -->
    <tr class="sep">
       <td colspan="<%= siteColumns %>"></td>
    </tr>
    
    <%
    foreach (var storeDashboard in Model.ExecutiveStoreDashboard.StoreList)
    {%>
     <tr>
        <!-- Store -->
        <td class="Col1">&nbsp;<strong><%=storeDashboard.Name %></strong></td>
        
        <!-- Sales -->
        <td class="ColOdd"><%= storeDashboard.Sales.ToString("N2")%></td>
        <td class="ColOdd"><%= storeDashboard.SalesVsLastWeek%>%</td>
        <td class="ColOdd"><%= storeDashboard.SalesVsLastYear%>%</td>
        
        <!-- Orders -->
        <td class="ColEven"><%= storeDashboard.Orders %></td>
        <td class="ColEven"><%= storeDashboard.OrdersVsLastWeek %>%</td>
        <td class="ColEven"><%= storeDashboard.OrdersVsLastYear %>%</td>
        
        <!-- Delivery -->
        <td class="ColOdd"><%= storeDashboard.LessThan30Min%>%</td>
        <td class="ColOdd"><%= storeDashboard.LessThan45Min%>%</td>
        
        <!-- Times -->
        <td class="ColEven"><%= storeDashboard.Make%>min</td>
        <td class="ColEven"><%= storeDashboard.OTD%>min</td>
        <td class="ColEven"><%= storeDashboard.TTD%>min</td>
        <td class="ColEven"><%= storeDashboard.AverageDriveTime%>min</td>
        
        <!-- Drivers -->
        <td class="ColOdd"><%= storeDashboard.Drivers%></td>
        <td class="ColOdd"><%= storeDashboard.DriversDelivering%></td>
        
        <!-- Misc -->
        <td class="ColEven"><%= storeDashboard.OPR.ToString("N1")%></td>        
        <td class="ColEven"><%= storeDashboard.AverageSpend.ToString("N2")%></td>
        
<%            
            string lastUpdatedClass = "ColOdd";

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
                else if (storeDashboard.LastUpdated.Value.Day == DateTime.Now.Day && storeDashboard.LastUpdated.Value.Month == DateTime.Now.Month)
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
%>
</table>
</div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ValidatorContent" runat="server">
</asp:Content>