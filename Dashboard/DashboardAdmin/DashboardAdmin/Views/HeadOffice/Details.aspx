<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardAdminViewData.IndexViewData>" %>
<%@ Import Namespace="DashboardAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink("Home", "/", "HeadOffice")%> > <%= Model.HeadOffice.HeadOfficeName %></h2>
     <table>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td><%= Html.ActionLink(Model.HeadOffice.Regions.Count + " Regions", "/Region/" + Model.HeadOffice.Id.Value, "HeadOffice")%></td><td><%= Html.ActionLink(Model.HeadOffice.Sites.Count + " Sites", "/Sites/" + Model.HeadOffice.Id.Value, "HeadOffice")%></td><td>TODO: <%= Model.HeadOffice.Language.Language.LanguageName %></td><td><%= Model.HeadOffice.DashboardUsers.Count%> users</td>
        </tr>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4"><strong>Dashboard Details</strong></td>
        </tr>
        <tr>
            <td><%= Html.ActionLink(Model.HeadOffice.IndicatorDefinitions.Count + " indicators", "/Dashboard/" + Model.HeadOffice.Id.Value, "HeadOffice")%></td><td></td><td></td><td></td>
        </tr>
     </table>

</asp:Content>
