<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroAdminViewData.IndexViewData>" %>
<%@ Import Namespace="AndroAdmin.Mvc.Extensions"%>
<%@ Import Namespace="AndroAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.Resource("Master, Translate")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "Index", "Home")%> > <%=Html.Resource("Master, Translate")%> > <%= Model.TranslatingProject %></h2>
   
    <table>
       <tr class="separator">
            <td colspan="5"></td>
        </tr>        
        <tr>
            <td><strong>Projects</strong></td>
        </tr> 
        <tr>
            <td><strong><%= Html.ActionLink("Andro Admin", "~/Index/Admin", "Translation")%></strong></td>
            <td><strong><%= Html.ActionLink("Order Tracking", "~/Index/OrderTracking", "Translation")%></strong></td>
            <td><strong><%= Html.ActionLink("Dashboard", "~/Index/Dashboard", "Translation")%></strong></td>
            <td><strong></strong></td>
        </tr>         
        <tr class="separator">
            <td colspan="5"></td>
        </tr>

        <tr>
            <td><strong>english</strong></td><td><strong>français</strong></td><td><strong>polski</strong></td><td><strong>български език</strong></td><td><strong>pусский язык</strong></td>
        </tr> 
         
    <%foreach (var b in Model.English)
      { %>

        <tr>
            <td><%= b.Value %></td><td><%= Html.ActionLink(Model.French[b.Key], "Translate/"+ Model.TranslatingProject +"/fr/" + b.Key, "Translation")%></td><td><%= Html.ActionLink(Model.Polish[b.Key], "Translate/" + Model.TranslatingProject + "/pl/" + b.Key, "Translation")%></td><td><%= Html.ActionLink(Model.Bulgarian[b.Key], "Translate/" + Model.TranslatingProject + "/bg/" + b.Key, "Translation")%></td><td><%= Html.ActionLink(Model.Russian[b.Key], "Translate/" + Model.TranslatingProject + "/ru/" + b.Key, "Translation")%></td>
        </tr> 
<%} %>
    <tr class="separator">
        <td colspan="5"></td>
    </tr>
</table>
</asp:Content>
