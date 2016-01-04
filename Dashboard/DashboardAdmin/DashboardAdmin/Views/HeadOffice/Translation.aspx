<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardAdminViewData.IndexViewData>" %>
<%@ Import Namespace="DashboardAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Translation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <h2><%= Html.ActionLink("Home", "/", "HeadOffice")%> > <%= Html.ActionLink(Model.HeadOffice.HeadOfficeName, "/Details/" + Model.HeadOffice.Id, "HeadOffice")%>  > <%= Html.ActionLink("Dashboard", "/Dashboard/" + Model.HeadOffice.Id, "HeadOffice")%> > Translation</h2>

<%
    using (Html.BeginForm("UpdateTranslation", "HeadOffice", FormMethod.Post))
    {
%> 

<%=Html.Hidden("IndicatorTranslation.Id", Model.IndicatorTranslation.Id)%>
<%=Html.Hidden("IndicatorTranslation.Language.Id", Model.IndicatorTranslation.Language.Id)%>
<%=Html.Hidden("IndicatorTranslation.IndicatorDefinition.Id", Model.IndicatorTranslation.IndicatorDefinition.Id)%>
<%=Html.Hidden("IndicatorTranslation.IndicatorDefinition.HeadOffice.Id", Model.IndicatorTranslation.IndicatorDefinition.HeadOffice.Id)%>
<table>
    <tr class="separator">
       <td colspan="4"></td>
    </tr> 
    <tr>
        <td><%= Model.IndicatorTranslation.IndicatorDefinition.IndicatorName %></td><td>Language</td><td></td><td></td>
    </tr>
    <tr>
        <td><%=Html.TextBox("IndicatorTranslation.TranslatedIndicatorName", Model.IndicatorTranslation.TranslatedIndicatorName)%></td><td>TODO:</td><td></td><td><input type="submit" value="Update" /></td>
    </tr>
</table>
<%
    }%>

</asp:Content>
