<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroAdminViewData.AdminViewData>" %>
<%@ Import Namespace="AndroAdmin.Mvc.Extensions"%>
<%@ Import Namespace="AndroAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, AddNewUser")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "Index", "Home")%> > <%= Html.ActionLink(Html.Resource("Master, Users"), "Index", "Admin")%> > <%=Html.Resource("Master, AddNewUser")%></h2>

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
            <td><%=Html.TextBox("FirstName")%></td><td><%=Html.TextBox("SurName")%></td><td><%=Html.CheckBox("Active")%> <%=Html.Resource("Master, Active")%></td><td></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, EmailAddress")%></td><td><%=Html.Resource("Master, Password")%></td><td></td>
        </tr>        
        <tr>
            <td><%=Html.TextBox("EmailAddress")%></td><td><%=Html.TextBox("Password")%></td><td></td><td><input type="submit" value="<%=Html.Resource("Master, CreateUser")%>" /></td>
        </tr>  
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
    </table>
<%} %>
</asp:Content>
