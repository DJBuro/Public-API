<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>
<%@ Import Namespace="WebDashboard.Web"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Sites
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Sites</h2>
    
    <table>
        <tr>
            <td><strong>Name</strong></td>
            <td><strong>Region</strong></td>
            <td><strong>Last Update</strong></td>
        </tr>
<%
    // Admin users geet to see and edit all sites
    if (Model.User.IsAdministrator)
    {
        foreach (Site site in Model.Sites)
        {
%>
        <tr>
            <td class="<%= site.Enabled %>"><%= Html.ActionLink(site.Name, "Site/" + site.Id, "Home") %></td>
            <td><%= site.Region.Name%></td>
            <td class="<%= (site.LastUpdated.Value.Day == DateTime.Now.Day && site.LastUpdated.Value.Month == DateTime.Now.Month)  ? "" : "Error" %>"><%= String.Format("{0:G}", site.LastUpdated)%></td>
        </tr>
<%    
        }
    }
    // Executive users get to see all stores dashboards
    else if (Model.User.IsExecutiveDashboardUser)
    {
        foreach (Site site in Model.Sites)
        {
%>
        <tr>
            <td class="<%= site.Enabled %>"><a href="/flex2/index.html#<%=Obfuscator.encryptString(site.Key.ToString())%>" target="_blank"><%= site.Name%></a></td>
            <td><%= site.Region.Name%></td>
            <td class="<%= (site.LastUpdated.Value.Day == DateTime.Now.Day && site.LastUpdated.Value.Month == DateTime.Now.Month)  ? "" : "Error" %>"><%= String.Format("{0:G}", site.LastUpdated)%></td>
        </tr>
<%    
        }
    }    
    // All other users can view dashboards but only for stores that they have been granted permission
    else 
    {
        foreach (Permission permission in Model.User.UserPermissions)
        {
            if (permission.Site.Enabled)
            {
%>
        <tr>
            <td class="<%= permission.Site.Enabled %>"><a href="/flex2/index.html#<%=Obfuscator.encryptString(permission.Site.Key.ToString())%>" target="_blank"><%= permission.Site.Name%></a></td>
            <td><%= permission.Site.Region.Name%></td>
            <td class="<%= (permission.Site.LastUpdated.Value.Day == DateTime.Now.Day && permission.Site.LastUpdated.Value.Month == DateTime.Now.Month)  ? "" : "Error" %>"><%= String.Format("{0:G}", permission.Site.LastUpdated)%></td>
        </tr>
<%
            }
        }
    }
%>
    </table>
</asp:Content>
