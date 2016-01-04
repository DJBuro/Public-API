<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	regional sites
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2><%=Html.ActionLink("Regions","Regions","Home") %> > <%=Model.Region.Name %></h2>
<table>
        <tr>
           <td colspan="2"><strong>Rameses Store Id</strong></td><td><strong>Name</strong></td><td><strong>Last Data Update</strong></td>
        </tr>
               <%
           foreach (var site in Model.Sites)
           {%>
        <tr>
           <td colspan="2" class="<%= site.Enabled %>"><%= site.SiteId%></td><td><%= Html.ActionLink(site.Name, "/Site/" + site.Id.Value, "Home")%></td><td class="<%= (site.LastUpdated.Value.Day == DateTime.Now.Day && site.LastUpdated.Value.Month == DateTime.Now.Month)  ? "" : "Error" %>"><%= String.Format("{0:G}", site.LastUpdated)%></td>
        </tr>
        <%
           }%> 
        </table>
</asp:Content>
