<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardAdminViewData.IndexViewData>" %>
<%@ Import Namespace="Dashboard.Dao.Domain"%>
<%@ Import Namespace="DashboardAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RegionSites
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink("Home", "/", "HeadOffice")%> > <%= Html.ActionLink(Model.Region.HeadOffice.HeadOfficeName, "/Details/" + Model.Region.HeadOffice.Id, "HeadOffice")%> > <%= Html.ActionLink("Regions", "/Region/" + Model.Region.HeadOffice.Id, "HeadOffice")%> > <%= Model.Region.RegionName %></h2>

<table>
    <tr class="separator">
           <td colspan="4"></td>
        </tr>
        <tr>
           <td><strong>Site Id</strong></td><td><strong>Name</strong></td><td><strong>IP</strong></td><td><strong>Enabled</strong></td>
        </tr>
               <%
           foreach (SitesRegion sitesRegion in Model.Region.SitesRegions)
           {%>
        <tr>
           <td><%= sitesRegion.Site.Id%></td><td><%= Html.ActionLink(sitesRegion.Site.SiteName, "/Site/" + sitesRegion.Site.Id.Value, "HeadOffice")%></td><td><%=sitesRegion.Site.IPAddress%></td><td class="<%=sitesRegion.Site.Enabled %>"></td>
        </tr>
        <%
           }%> 
        </table>


</asp:Content>
