<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, Regions")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "HeadOffice")%> > <%= Html.ActionLink(Model.HeadOffice.Name, "/Details/" + Model.HeadOffice.Id, "HeadOffice")%>  > <%=Html.Resource("Master, Regions")%></h2>

<table>
    <tr class="separator">
        <td colspan="4"></td>
    </tr>
        <%     
            SortedList<string, string> timeZones = new SortedList<string, string>();
            System.Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo> zones = TimeZoneInfo.GetSystemTimeZones();

            foreach (TimeZoneInfo zone in zones)
            {
                string displayText = zone.Id + " (UTC";

                if (zone.BaseUtcOffset.TotalHours > 0)
                {
                    displayText += "+";
                }

                displayText += zone.BaseUtcOffset.TotalHours.ToString();

                if (zone.SupportsDaylightSavingTime)
                {
                    displayText += ", daylight";
                }

                displayText += ")";

                timeZones.Add(zone.Id, displayText);
            }
            
    using (Html.BeginForm("AddRegion", "HeadOffice", FormMethod.Post))
{
%>
<%=Html.Hidden("Region.HeadOffice.Id", Model.HeadOffice.Id)%>
    <tr>
        <td colspan="4"><%=Html.Resource("Master, AddNewRegion")%></td>
    </tr>
    <tr>
        <td><%=Html.TextBox("Region.Name","", new { @size = "40" })%></td>
        <td><input type="submit" value="<%=Html.Resource("Master, Add")%>" /></td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td>
            <select id="Region_TimeZone" name="Region.TimeZone">
<%
                foreach (KeyValuePair<string, string> keyValuePair in timeZones)
                {
                    %> 
                    <option value="<%= keyValuePair.Key%>"><%= keyValuePair.Value%></option>
                    <%
                }
%>
            </select>
       </td>
       <td></td>
       <td></td>
       <td></td>
    </tr>
     <tr class="separator">
       <td colspan="4"></td>
    </tr>
<%
    }%>
    <tr>
        <td><%=Html.Resource("Master, Name")%></td><td></td><td></td><td><%=Html.Resource("Master, Sites")%></td>
    </tr>
<%
    foreach (Region region in Model.HeadOffice.Regions)
    {
        using (Html.BeginForm("UpdateRegion", "HeadOffice", FormMethod.Post))
        {
%> 
   <%=Html.Hidden("Region.Id", region.Id)%>
   <%=Html.Hidden("Region.HeadOffice.Id", region.HeadOffice.Id)%>

    <tr>
        <td><%=Html.TextBox("Region.Name", region.Name, new { @size = "40" })%></td>
        <td><input type="submit" value="<%=Html.Resource("Master, Update")%>" /></td>
        <td></td>
        <td><%= Html.ActionLink(region.RegionalSites.Count + "","RegionalSites/" + region.Id.Value,"HeadOffice") %></td>

    </tr>
    <tr>
        <td>
            <select id="Region_Timezone" name="TimeZone">
<%
                foreach (KeyValuePair<string, string> keyValuePair in timeZones)
                {
                    %> 
                    <option value="<%= keyValuePair.Key%>" <%= region.TimeZone == keyValuePair.Key ? " selected=selected " : ""  %>><%= keyValuePair.Value%></option>
                    <%
                }
%>
            </select>
       </td>
       <td></td>
       <td></td>
       <td></td>
    </tr>
     <tr class="separator">
       <td colspan="4"></td>
    </tr>

<%
        } //end form
    
    }//end foreach loop

    foreach (Region region in Model.HeadOffice.Regions)
        {
            if(region.RegionalSites.Count == 0)
            {
               using (Html.BeginForm("RemoveRegion", "HeadOffice", FormMethod.Post))
                {
%>
   <%=Html.Hidden("Region.Id", region.Id)%>
   <%=Html.Hidden("Region.HeadOffice.Id", region.HeadOffice.Id)%>
   <%=Html.Hidden("Region.Name", region.Name)%>
    <tr>
        <td colspan="4"><%=Html.Resource("Master, RemoveRegion")%></td>
    </tr>
    <tr>
        <td><strong><%=region.Name%></strong></td><td><input type="submit" value="<%=Html.Resource("Master, RemoveRegion")%>" /></td><td></td><td></td>
    </tr>
     <tr class="separator">
       <td colspan="4"></td>
    </tr>
    <%
    } 
                
            }

        }    
        
%>

</table>



</asp:Content>