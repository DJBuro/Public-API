<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroAdminViewData.IndexViewData>" %>
<%@ Import Namespace="AndroAdmin.Mvc.Extensions"%>
<%@ Import Namespace="AndroAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Translate
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "Index", "Home")%> > <%= Html.ActionLink("Translate", "Index", "Translation")%> > <%= Html.ActionLink(Model.TranslatingProject, "Index/" + Model.TranslatingProject, "Translation")%> > <%= Model.TranslatingLanguage %></h2>

<table>
    <tr class="separator">
        <td colspan="3"></td>
    </tr>
    <tr>
        <td><strong>english</strong></td><td><%= Model.EnglishWord %></td><td></td>
    </tr>
        <%
            using (Html.BeginForm("Save", "Translation", FormMethod.Post))
            {
%>    
<input id="translatingId" name="translatingWordId" type="hidden" value="<%=Model.TranslatingWordId%>" />
<input id="translatingProject" name="translatingProject" type="hidden" value="<%=Model.TranslatingProject%>" />
<input id="translatingLanguageId" name="translatingLanguageId" type="hidden" value="<%=Model.TranslatingLanguageId%>" />
    <tr>
        <td><strong><%= Model.TranslatingLanguage%></strong></td><td><%=Html.TextBox("TranslatedWord", Model.TranslatedWord)%></td><td><input type="submit" value="Save" /></td>
    </tr>
    <%} %>
   <tr class="separator">
        <td colspan="3"></td>
    </tr>
</table>    




</asp:Content>
