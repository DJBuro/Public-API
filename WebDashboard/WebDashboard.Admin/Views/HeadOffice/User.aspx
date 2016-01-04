<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, Users")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "HeadOffice")%> > <%= Html.ActionLink(Model.HeadOffice.Name, "/Details/" + Model.HeadOffice.Id, "HeadOffice")%>  > <%= Html.ActionLink("Users", "/Users/" + Model.HeadOffice.Id, "HeadOffice")%> > <%=Html.Resource("Master, Users")%></h2>

<%
    using (Html.BeginForm("UpdateUser", "HeadOffice", FormMethod.Post))
    {
%> 
<table>
<%=Html.Hidden("User.Id", Model.User.Id)%>

    <tr class="separator">
       <td colspan="6"></td>
    </tr> 
    <tr>
       <td colspan="6"><strong><%=Html.Resource("Master, EditUser")%></strong></td>
    </tr> 
     <tr>
        <td><%=Html.Resource("Master, EmailAddress")%></td>
        <td style="text-align:center;"><%=Html.Resource("Master, StoreUser")%></td>
        <td style="text-align:center;"><%=Html.Resource("Master, HeadOfficeUser")%></td>
        <td style="text-align:center;"><%=Html.Resource("Master, AdminUser")%></td>
        <td style="text-align:center;"><%=Html.Resource("Master, Enabled")%></td>
    </tr>
    <tr>
        <td><%=Html.TextBox("EmailAddress", Model.User.EmailAddress, new { @size = "30" })%></td>
        <td style="text-align:center;"><%=Html.CheckBox("User.StoreUser", Model.User.StoreUser)%></td>
        <td style="text-align:center;"><%=Html.CheckBox("User.IsExecutiveDashboardUser", Model.User.IsExecutiveDashboardUser)%></td>
        <td style="text-align:center;"><%=Html.CheckBox("User.IsAdministrator", Model.User.IsAdministrator)%></td>
        <td style="text-align:center;"><%=Html.CheckBox("User.Active", Model.User.Active)%></td>
    </tr>
    <tr>
        <td><%=Html.Resource("Master, Password")%></td>
        <td colspan="5"></td>
    </tr>
    <tr>
        <td><%=Html.TextBox("Password", Model.User.Password, new { @size = "30" })%></td>
        <td colspan="5"></td>
    </tr>
    <tr>
        <td colspan="5"></td>
        <td><input type="submit" value="<%=Html.Resource("Master, Update")%>" /></td>
    </tr>
    <%
    }
        if(!Model.User.Active)
        {
                using (Html.BeginForm("RemoveUser", "HeadOffice", FormMethod.Post))
                {
                   %>
                   <%=Html.Hidden("Id", Model.User.Id)%>
     <tr>
        <td colspan="2"></td><td><input type="submit" value="<%=Html.Resource("Master, Remove")%>" /></td>
    </tr>                  
                   
                   <% 
                }
        }
%>

    <tr class="separator">
       <td colspan="3"></td>
    </tr>
    <tr>
       <td colspan="2"><strong><%=Html.Resource("Master, UserPermissions")%></strong></td><td><%= Html.ActionLink(Html.Resource("Master, EditUserPermissions"), "/Permissions/" + Model.User.Id.Value, "HeadOffice")%></td>
    </tr>
    <%
        foreach (Permission permission in Model.User.UserPermissions)
        { 
     %>
    <tr>
        <td colspan="2"><%= permission.Site.Name %></td><td></td>
    </tr>
<%
        } %>
</table>

</asp:Content>
