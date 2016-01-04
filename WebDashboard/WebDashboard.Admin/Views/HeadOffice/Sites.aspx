<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, Sites")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "HeadOffice")%> > <%= Html.ActionLink(Model.HeadOffice.Name, "/Details/" + Model.HeadOffice.Id, "HeadOffice")%>  > <%=Html.Resource("Master, Sites")%></h2>

<%
    using (Html.BeginForm("CreateSite", "HeadOffice", FormMethod.Post))
    {
%> 

<%=Html.Hidden("Site.HeadOffice.Id", Model.HeadOffice.Id)%>
<table>
    <tr class="separator">
       <td colspan="5"></td>
    </tr> 
    <tr>
       <td colspan="5"><strong><%=Html.Resource("Master, AddNewSite")%></strong></td>
    </tr> 
     <tr>
        <td colspan="2"><%=Html.Resource("Master, Name")%></td><td><%=Html.Resource("Master, Region")%></td><td><%=Html.Resource("Master, SiteType")%></td><td></td>
    </tr>
    <tr>
        <td colspan="2"><%=Html.TextBox("Site.Name","", new { @size = "40" })%></td><td><%=Html.DropDownList("Site.Region.Id", Model.RegionListItems)%></td><td><%=Html.DropDownList("Site.SiteType.Id", Model.SiteTypeListItems)%></td><td></td>
    </tr>
    <tr>
        <td><%=Html.Resource("Master, RamesesStoreId")%></td><td><%=Html.Resource("Master, IPAddress")%></td><td><%=Html.Resource("Master, SiteKey")%></td><td></td>
    </tr>
    <tr>
        <td><%=Html.TextBox("Site.SiteId", "", new { @size = "10" })%></td><td><%=Html.TextBox("Site.IPAddress")%></td><td><%=Html.TextBox("Site.Key","", new { @size = "10" })%></td><td colspan="2"><%=Html.CheckBox("Site.Enabled")%> <%=Html.Resource("Master, Enabled")%></td>
    </tr>
    <tr>
        <td colspan="4"></td><td><input type="submit" value="<%=Html.Resource("Master, AddNewSite")%>" /></td>
    </tr>
    
    <%
    }
%>
    <tr class="separator">
           <td colspan="5"></td>
        </tr>
        <tr>
           <td><strong><%=Html.Resource("Master, RamesesStoreId")%></strong></td><td><strong><%=Html.Resource("Master, Name")%></strong></td><td><strong><%=Html.Resource("Master, LastUpdate")%></strong></td><td><strong><%=Html.Resource("Master, Region")%></strong></td><td><strong><%=Html.Resource("Master, Enabled")%></strong></td>
        </tr>
        <%
           foreach (Site site in Model.HeadOffice.Sites)
           { 
         %>
        <tr>
           <td><%= site.SiteId %></td><td><%= Html.ActionLink(site.Name, "/Site/" + site.Id.Value, "HeadOffice")%></td><td class="<%= site.LastUpdated.Value.Day == DateTime.Now.Day ? "" : "error" %>"><%= String.Format("{0:G}", site.LastUpdated)%></td><td><%= Html.ActionLink(site.Region.Name, "/RegionalSites/" + site.Region.Id, "HeadOffice")%></td><td class="<%= site.Enabled %>"></td>
        </tr>
        <%
           }%> 
</table>

</asp:Content>
