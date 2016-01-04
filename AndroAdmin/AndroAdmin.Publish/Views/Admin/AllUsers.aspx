<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroAdminViewData.AdminViewData>" %>
<%@ Import Namespace="AndroAdmin.Mvc.Extensions"%>
<%@ Import Namespace="AndroAdmin.Dao.Domain"%>
<%@ Import Namespace="AndroAdmin.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, Users")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "Index", "Home")%> > <%=Html.Resource("Master, Users")%></h2>
    <table>        
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td></td><td></td><td><%=Html.Resource("Master, Created")%></td><td><%=Html.Resource("Master, Active")%></td>
        </tr>  
    <%
        foreach (var androUser in Model.AndroUsers)
        {%>

         <tr>
            <td><strong><%= Html.ActionLink(androUser.FirstName + " " + androUser.SurName, "EditUser/" + androUser.Id.Value, "Admin")%></strong></td><td><%= androUser.EmailAddress%></td><td><%= String.Format("{0:f}", androUser.Created)%></td><td class="<%= androUser.Active %>"></td>
        </tr>          
        <% }%>    
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
         <tr>
            <td><%= Html.ActionLink(Html.Resource("Master, AddNewUser"), "AddUser", "Admin")%></td><td></td><td></td><td></td>
        </tr>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
    </table>
</asp:Content>
