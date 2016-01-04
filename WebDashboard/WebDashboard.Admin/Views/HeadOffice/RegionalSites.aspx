<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, RegionalStores")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "HeadOffice")%> > <%= Html.ActionLink(Model.Region.HeadOffice.Name, "/Details/" + Model.Region.HeadOffice.Id, "HeadOffice")%> > <%= Html.ActionLink(Html.Resource("Master, Regions"), "/Region/" + Model.Region.HeadOffice.Id, "HeadOffice")%> > <%= Model.Region.Name %></h2>

<table>
    <tr class="separator">
           <td colspan="4"></td>
        </tr>
        <tr>
           <td><strong><%=Html.Resource("Master, RamesesStoreId")%></strong></td><td><strong><%=Html.Resource("Master, Name")%></strong></td><td><strong><%=Html.Resource("Master, LastUpdate")%></strong></td><td><strong><%=Html.Resource("Master, Enabled")%></strong></td>
        </tr>
               <%
           foreach (var site in Model.Sites)
           {%>
        <tr>
           <td><%= site.SiteId%></td><td><%= Html.ActionLink(site.Name, "/Site/" + site.Id.Value, "HeadOffice")%></td><td><%= String.Format("{0:G}", site.LastUpdated)%></td><td class="<%= site.Enabled %>"></td>
        </tr>
        <%
           }%> 
        </table>


</asp:Content>
