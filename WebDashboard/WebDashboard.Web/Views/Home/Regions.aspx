<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	regions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Regions</h2>
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
    
    using (Html.BeginForm("Regions", "Home", FormMethod.Post, new { @id = "form1" }))
    {
%>
<table>
    <%=Html.Hidden("Region.HeadOffice.Id", Model.HeadOffice.Id)%>
    <tr>
        <td colspan="3"><strong>Add New Region</strong></td>
    </tr>
    <tr>
        <td>Region Name:</td>
        <td><%=Html.TextBox("Region.Name", "", new { @size = "40", @class = "{validate:{required:true,minlength:3,messages:{required:'Please a Region Name'}}}" })%></td>
        <td></td>
    </tr>
    <tr>
        <td>TimeZone:</td>
        <td>
            <select id="TimeZone" name="TimeZone">
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
       <td><input type="submit" value="Add" /></td>
    </tr>
    <tr class="separator">
       <td colspan="3"></td>
    </tr>
<%
    }//end add form%>
    <tr>
        <td><strong><%= Model.User.HeadOffice.Name %> Regions</strong></td>
        <td></td>
        <td><strong>Regional Sites</strong></td>
    </tr>
    <%
    foreach (Region region in Model.HeadOffice.Regions)
    {
        using (Html.BeginForm("UpdateRegion", "Home", FormMethod.Post))
        {
%> 
        <input id="Id" name="Id" type="hidden" value="<%=region.Id%>" />
        <input id="HeadOffice.Id" name="HeadOffice.Id" type="hidden" value="<%=region.HeadOffice.Id%>" />
        <tr>   
            <td>Name:</td>
            <td>
                <input id="Name" name="Name" type="text" value="<%= region.Name%>" size="40" />
            </td>
            <td><%= Html.ActionLink(region.RegionalSites.Count + "","RegionalSites/" + region.Id.Value,"Home") %></td>
        </tr>
        <tr>
            <td>TimeZone:</td>
            <td> 
                <select id="TimeZone" name="TimeZone">
<%
                    foreach (KeyValuePair<string, string> keyValuePair in timeZones)
                    {
                        if (region.TimeZone == keyValuePair.Key)
                        {
                            %> 
                            <option selected="selected" value="<%= keyValuePair.Key%>"><%= keyValuePair.Value%></option>
                            <%
                        }
                        else
                        {
                            %> 
                            <option value="<%= keyValuePair.Key%>"><%= keyValuePair.Value%></option>
                            <%
                        }
                    }
%>
                </select>
            </td>
            <td><input type="submit" value="Update" /></td>
        </tr>
        
    <tr class="separatorLite">
       <td colspan="3"></td>
    </tr>
<%
        } //end form
    }//end foreach loop
%>
    <tr class="separator">
       <td colspan="3"></td>
    </tr>
<%
    foreach (Region region in Model.HeadOffice.Regions)
        {
            if(region.RegionalSites.Count == 0)
            {
               using (Html.BeginForm("RemoveRegion", "Home", FormMethod.Post))
                {
%>
   <%=Html.Hidden("Id", region.Id)%>
    <tr>
        <td colspan="3">Remove Region</td>
    </tr>
    <tr>
        <td><strong><%=region.Name%></strong></td>
        <td></td>
        <td><input type="submit" value="Remove Region" /></td>
    </tr>
     <tr class="separator">
       <td colspan="3"></td>
    </tr>
    <%
    } //end form
            }//endif
        }//end foreach    
        
%>
</table>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HelpContent" runat="server">
                        <li>You cannot delete a region if it has sites</li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ValidatorContent" runat="server">
<script type="text/javascript">
    $(document).ready(function() {

        var container = $('div.container');

        var validator = $("#form1").validate({
            errorContainer: container,
            errorLabelContainer: $("ul", container),
            wrapper: 'li',
            meta: "validate"
        });
    });
</script>
</asp:Content>