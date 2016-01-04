<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardAdminViewData.IndexViewData>" %>
<%@ Import Namespace="Dashboard.Dao.Domain"%>
<%@ Import Namespace="DashboardAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Sites
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink("Home", "/", "HeadOffice")%> > <%= Html.ActionLink(Model.HeadOffice.HeadOfficeName, "/Details/" + Model.HeadOffice.Id, "HeadOffice")%>  > Sites</h2>

<table>
    <tr class="separator">
           <td colspan="5"></td>
        </tr>
        <tr>
           <td><strong>Site Id</strong></td><td><strong>Name</strong></td><td><strong>IP</strong></td><td><strong>Region</strong></td><td><strong>Enabled</strong></td>
        </tr>
               <%
           foreach (Site site in Model.HeadOffice.Sites)
           { 
               SitesRegion sitesRegion = new SitesRegion();

               if (site.SitesRegions.Count == 1)
               {
                   sitesRegion = (SitesRegion)site.SitesRegions[0];
               }
               else
               {
                   sitesRegion.Region = new Region();
                   sitesRegion.Region.Id = 0;
                   sitesRegion.Region.RegionName = "none";
               }

               %>
        <tr>
           <td><%= site.Id %></td><td><%= Html.ActionLink(site.SiteName, "/Site/" + site.Id.Value, "HeadOffice")%></td><td><%=site.IPAddress %></td><td><%= Html.ActionLink(sitesRegion.Region.RegionName, "/RegionSites/" + sitesRegion.Region.Id, "HeadOffice")%></td><td class="<%= site.Enabled %>"></td>
        </tr>
        <%
           }%> 
        </table>

</asp:Content>
