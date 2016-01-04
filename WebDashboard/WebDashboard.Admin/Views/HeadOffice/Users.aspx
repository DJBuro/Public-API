<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2><%= Html.ActionLink("Home", "/", "HeadOffice")%> > <%= Html.ActionLink(Model.HeadOffice.Name, "/Details/" + Model.HeadOffice.Id, "HeadOffice")%>  > Users</h2>

<%
    using (Html.BeginForm("CreateUser", "HeadOffice", FormMethod.Post))
    {
%> 

<%=Html.Hidden("User.HeadOffice.Id", Model.HeadOffice.Id)%>
<table>
    <tr class="separator">
       <td colspan="5"></td>
    </tr> 
    <tr><!-- TODO: culture needs doing on this page -->
       <td colspan="5"><strong>Add New User</strong></td>
    </tr> 
     <tr>
        <td><%=Html.Resource("Master, EmailAddress")%></td>
        <td style="text-align:center;"><%=Html.Resource("Master, StoreUser")%></td>
        <td style="text-align:center;"><%=Html.Resource("Master, HeadOfficeUser")%></td>
        <td style="text-align:center;"><%=Html.Resource("Master, AdminUser")%></td>
        <td style="text-align:center;">Active</td>
    </tr>
    <tr>
        <td><%=Html.TextBox("User.EmailAddress", "", new { @size = "30" })%></td>
        <td style="text-align:center;"><%=Html.CheckBox("User.StoreUser")%></td>
        <td style="text-align:center;"><%=Html.CheckBox("User.IsExecutiveDashboardUser")%></td>
        <td style="text-align:center;"><%=Html.CheckBox("User.IsAdministrator")%></td>
        <td style="text-align:center;"><%=Html.CheckBox("User.Active")%></td>
    </tr>
    <tr>
        <td>Password</td>
        <td colspan="4"></td>
    </tr>
    <tr>
        <td><%=Html.TextBox("User.Password", "", new { @size = "30" })%></td>
        <td colspan="4"></td>
    </tr>
    <tr>
        <td colspan="4"></td>
        <td><input type="submit" value="Add User" /></td>
    </tr>
</table>
    <%
    }
%>
<table>
    <tr class="separator">
       <td colspan="5"></td>
    </tr>
    <tr>
       <td><strong>Users</strong></td>
       <td style="text-align:center; width:65px;"><strong><%=Html.Resource("Master, StoreUser")%></strong></td>
       <td style="text-align:center; width:65px;"><strong><%=Html.Resource("Master, HeadOfficeUser")%></strong></td>
       <td style="text-align:center; width:65px;"><strong><%=Html.Resource("Master, AdminUser")%></strong></td>
       <td style="text-align:center; width:65px;"><strong><%=Html.Resource("Master, Enabled")%></strong></td>
    </tr>
    <%
       foreach (User user in Model.HeadOffice.Users)
       { 
     %>
    <tr>
        <td><%= Html.ActionLink(user.EmailAddress, "/User/" + user.Id.Value, "HeadOffice")%></td>
        <td style="text-align:center;" class="<%= user.StoreUser %>"></td>
        <td style="text-align:center;" class="<%= user.IsExecutiveDashboardUser %>"></td>
        <td style="text-align:center;" class="<%= user.IsAdministrator %>"></td>
        <td style="text-align:center;" class="<%= user.Active %>"></td>
    </tr>
        <%
           }%> 
</table>
</asp:Content>
