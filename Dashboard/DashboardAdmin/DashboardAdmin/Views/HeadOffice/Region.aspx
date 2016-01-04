<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardAdminViewData.IndexViewData>" %>
<%@ Import Namespace="Dashboard.Dao.Domain"%>
<%@ Import Namespace="DashboardAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Region
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<h2><%= Html.ActionLink("Home", "/", "HeadOffice")%> > <%= Html.ActionLink(Model.HeadOffice.HeadOfficeName, "/Details/" + Model.HeadOffice.Id, "HeadOffice")%>  > Regions</h2>

<table>
    <tr class="separator">
        <td colspan="4"></td>
    </tr>
    <tr>
        <td>Name</td><td></td><td></td><td>Sites</td>
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
        <td><%=Html.TextBox("Region.RegionName", region.RegionName)%></td><td><input type="submit" value="Update" /></td><td></td><td><%= Html.ActionLink(region.SitesRegions.Count.ToString(), "/RegionSites/" + region.Id, "HeadOffice")%></td>
    </tr>
     <tr class="separator">
       <td colspan="4"></td>
    </tr>

<%
        } //end form
    
    }//end foreach loop
%>
    <%     
        using (Html.BeginForm("AddRegion", "HeadOffice", FormMethod.Post))
    {
%>
<%=Html.Hidden("Region.HeadOffice.Id", Model.HeadOffice.Id)%>
    <tr>
        <td colspan="4">Add Region</td>
    </tr>
    <tr>
        <td><%=Html.TextBox("Region.RegionName")%></td><td><input type="submit" value="Add" /></td><td></td><td></td>
    </tr>
     <tr class="separator">
       <td colspan="4"></td>
    </tr>
<%
    }%>

    <%


        foreach (Region region in Model.HeadOffice.Regions)
        {
            if(region.SitesRegions.Count == 0)
            {
                
               using (Html.BeginForm("RemoveRegion", "HeadOffice", FormMethod.Post))
                {

%>
   <%=Html.Hidden("Region.Id", region.Id)%>
   <%=Html.Hidden("Region.HeadOffice.Id", region.HeadOffice.Id)%>
   <%=Html.Hidden("Region.RegionName", region.RegionName)%>
    <tr>
        <td colspan="4">Remove Region</td>
    </tr>
    <tr>
        <td><strong><%=region.RegionName%></strong></td><td><input type="submit" value="Remove" /></td><td></td><td></td>
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
