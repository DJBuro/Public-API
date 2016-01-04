<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"Inherits="System.Web.Mvc.ViewPage<DashboardAdminViewData.IndexViewData>" %>
<%@ Import Namespace="DashboardAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add Site Region
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>HeadOffice > Add New Site > Add Site Region</h2>
<%
    using (Html.BeginForm("AddSiteRegion", "HeadOffice", FormMethod.Post))
    {
%> 
<%=Html.Hidden("SitesRegion.Site.Id", Model.Site.Id)%>
<table>
    <tr class="separator">
       <td colspan="4"></td>
    </tr> 
     <tr>
        <td>Head Office</td><td></td><td></td><td></td>
    </tr>
    <tr>
        <td><%=Html.DropDownList("SitesRegion.Region.Id", Model.RegionListItems)%></td><td></td><td></td><td></td>
    </tr>
    <tr>
        <td colspan="3"></td><td><input type="submit" value="Save" /></td>
    </tr>
    <%
    }
%>

    </table>
</asp:Content>
