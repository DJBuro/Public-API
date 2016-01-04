<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroAdminViewData.AdminViewData>" %>
<%@ Import Namespace="AndroAdmin.Mvc.Extensions"%>
<%@ Import Namespace="AndroAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditUser
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2><%= Html.ActionLink(Html.Resource("Master, Home"), "Index", "Home")%> > <%= Html.ActionLink("Users", "Index", "Admin")%> > <%= Model.AndroEditUser.FirstName%> <%= Model.AndroEditUser.SurName%></h2>
    <%
        using (Html.BeginForm("Permissions", "Admin", FormMethod.Post))
        {
%>    
<input id="Id" name="Id" type="hidden" value="<%=Model.AndroEditUser.Id%>" />
    <table>        
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, FirstName")%></td><td><%=Html.Resource("Master, SurName")%></td><td></td><td></td>
        </tr>        
        <tr>
            <td><%=Html.TextBox("FirstName", Model.AndroEditUser.FirstName)%></td><td><%=Html.TextBox("SurName", Model.AndroEditUser.SurName)%></td><td><%=Html.CheckBox("Active", Model.AndroEditUser.Active)%> Active</td><td></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, EmailAddress")%></td><td><%=Html.Resource("Master, Password")%></td><td></td><td></td>
        </tr>        
        <tr>
            <td><%=Html.TextBox("EmailAddress", Model.AndroEditUser.EmailAddress)%></td><td><%=Html.TextBox("Password", Model.AndroEditUser.Password)%></td><td></td><td><input type="submit" value="<%=Html.Resource("Master, UpdateUser")%>" /></td>
        </tr>  
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <% if (!Model.AndroEditUser.Active)
           {%>
        <tr>
            <td><%= Html.ActionLink(Html.Resource("Master, DeleteUser"), "DeleteUser/" + Model.AndroEditUser.Id, "Admin")%></td><td></td><td></td><td></td>
        </tr>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <%} %>
    </table>
<%} %>

</asp:Content>
