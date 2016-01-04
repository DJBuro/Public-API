<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroAdminViewData.IndexViewData>" %>
<%@ Import Namespace="AndroAdmin.Mvc.Extensions"%>
<%@ Import Namespace="AndroAdmin.Models"%>
<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Andro Web <%=Html.Resource("Master, Administration")%>
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Resource("Master, WelcomeTo")%> Andro Web <%=Html.Resource("Master, Administration")%></h2>
    <table>
      <tr class="separator">
            <td></td>
        </tr> 
<%
    using (Html.BeginForm("Login", "Home", FormMethod.Post))
    {
%>
        <tr>
            <td><%=Html.Resource("Master, EmailAddress")%></td>
        </tr>
        <tr>
            <td><%= Html.TextBox("emailAddress","", new { @size = "40" })%></td>
        </tr>    
        <tr>
            <td><%=Html.Resource("Master, Password")%></td>
        </tr>
        <tr>
            <td><%= Html.Password("password","", new { @size = "40" })%></td>
        </tr> 
        <tr>
            <td><input type="submit" value="<%=Html.Resource("Master, LogIn")%>" /></td>
        </tr> 
        <tr class="separator">
            <td></td>
        </tr> 
        
<%
    }
%>
    </table>
</asp:Content>
