<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardAdminViewData.IndexViewData>" %>
<%@ Import Namespace="Dashboard.Dao.Domain"%>
<%@ Import Namespace="DashboardAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Site
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2><%= Html.ActionLink("Home", "/", "HeadOffice")%> > <%= Html.ActionLink(Model.Site.HeadOffice.HeadOfficeName, "/Details/" + Model.Site.HeadOffice.Id, "HeadOffice")%> > <%= Html.ActionLink("Sites", "/Sites/" + Model.Site.HeadOffice.Id, "HeadOffice")%>  > <%= Model.Site.SiteName%></h2>

<%
    using (Html.BeginForm("UpdateSite", "HeadOffice", FormMethod.Post))
    {
%> 
<%=Html.Hidden("Site.Id", Model.Site.Id)%>
<%=Html.Hidden("Site.HeadOffice.Id", Model.Site.HeadOffice.Id)%>
<table>
    <tr class="separator">
       <td colspan="4"></td>
    </tr> 
    <tr>
        <td colspan="4"><strong>Site Details</strong></td>
    </tr>
    <tr>
        <td>Rameses Site Id</td><td>Name</td><td>IP Address</td><td></td>
    </tr>
    <tr>
        <td><%=Html.TextBox("Site.Id", Model.Site.Id)%></td><td><%=Html.TextBox("Site.SiteName", Model.Site.SiteName)%></td><td><%=Html.TextBox("Site.IPAddress", Model.Site.IPAddress)%></td><td><%=Html.CheckBox("Site.Enabled", Model.Site.Enabled)%> Enabled</td>
    </tr>
    <tr>
        <td colspan="3"></td><td><input type="submit" value="Update" /></td>
    </tr>
     <tr class="separator">
       <td colspan="4"></td>
    </tr>
    <%
    }
    using (Html.BeginForm("UpdateSiteRegion", "HeadOffice", FormMethod.Post))
    {
        var siteRegion =  new SitesRegion();;

        if (Model.Site.SitesRegions.Count == 1)
        {
            siteRegion = (SitesRegion) Model.Site.SitesRegions[0];
        }
        else
        {
            siteRegion.Region = new Region();
        }

%>

<%=Html.Hidden("SitesRegion.Id", siteRegion.Id)%>
<%=Html.Hidden("SitesRegion.Site.Id", Model.Site.Id)%>
    <tr>
        <td colspan="4"><strong>Site Region</strong></td>
    </tr>
    <tr>
        <td><%=Html.DropDownList("SitesRegion.Region.Id", Model.RegionListItems)%></td><td></td><td></td><td><input type="submit" value="Update" /></td>
    </tr>
    <tr class="separator">
       <td colspan="4"></td>
    </tr>
    <%
    }//end update siteRegion

    //you cannot remove an active site
    if (!Model.Site.Enabled)
    {
        using (Html.BeginForm("RemoveSite", "HeadOffice", FormMethod.Post))
        {
%> 
        <%=Html.Hidden("Site.Id", Model.Site.Id)%>
        <%=Html.Hidden("Site.HeadOffice.Id", Model.Site.HeadOffice.Id)%>
    <tr>
        <td colspan="3"><strong>Remove Site</strong></td><td><input type="submit" value="Remove" /></td>
    </tr>
        <tr class="separator">
        <td colspan="4"></td>
    </tr>
    <%
    }//end removeSite
    }//end if%>

</table>

</asp:Content>
