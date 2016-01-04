<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<DashboardViewData.DisplayViewData>" %>
<%@ Import Namespace="Dashboard.Dao.Domain"%>
<%@ Import Namespace="Dashboard.Web.WebSite.Models"%>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Administration Area
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="siteMessage">
    <strong><%= Model.HeadOfficeMessage %></strong>
</div>


 <%foreach (var dd in Model.Regions)
       
      {%>
      <div class="regionalColumns">
        <table>
            <tr>
                <td><strong><%=Html.ActionLink(dd.RegionName, "Region/" + dd.Id.Value.ToString())%></strong></td><td align="center"><strong>Enabled</strong></td>
            </tr>
            <%foreach (SitesRegion sr in dd.SitesRegions)
              {%> 
        <tr>
                <td><%=Html.ActionLink(sr.Site.SiteName, "Site/" + sr.Site.Id.Value.ToString()) %></td><td align="center"><%= (sr.Site.Enabled) ? "" :"NO" %></td>
            </tr>
            <%
            }%>
        </table>
      </div>
  <%} %>


  
</asp:Content>