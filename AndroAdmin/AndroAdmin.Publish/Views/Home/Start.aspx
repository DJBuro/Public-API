<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroAdminViewData.IndexViewData>" %>
<%@ Import Namespace="AndroAdmin.Mvc.Extensions"%>
<%@ Import Namespace="AndroAdmin.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Andro Web <%=Html.Resource("Master, Administration")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Resource("Master, Home")%></h2>
    <table>        
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4"><strong><%= Html.Resource("Master, WelcomeTo")%> Andro Web <%=Html.Resource("Master, Administration")%> <%= Model.AndroUser.FirstName %> <%= Model.AndroUser.SurName %></strong></td>
        </tr>    
        <tr>
            <td colspan="4">-<%=Html.Resource("Master, Enjoy")%></td>
        </tr>  
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
    </table>
</asp:Content>
