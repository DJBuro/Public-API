<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardAdminViewData.IndexViewData>" %>
<%@ Import Namespace="DashboardAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add New Site
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink("Home", "/", "HeadOffice")%> > Add New Site</h2>
<%
    using (Html.BeginForm("CreateSite", "HeadOffice", FormMethod.Post))
    {
%> 
<table>
    <tr class="separator">
       <td colspan="4"></td>
    </tr> 
     <tr>
        <td>Head Office</td><td></td><td></td><td></td>
    </tr>
    <tr>
        <td><%=Html.DropDownList("Site.HeadOffice.Id", Model.HeadOfficeListItems)%></td><td></td><td></td><td></td>
    </tr>
    <tr>
        <td>Rameses Site Id</td><td>Name</td><td>IP Address</td><td></td>
    </tr>
    <tr>
        <td><%=Html.TextBox("Site.Id")%></td><td><%=Html.TextBox("Site.SiteName")%></td><td><%=Html.TextBox("Site.IPAddress")%></td><td><%=Html.CheckBox("Site.Enabled")%> Enabled</td>
    </tr>
    <tr>
        <td colspan="3"></td><td><input type="submit" value="Save" /></td>
    </tr>
    
    <%
    }
%>

    </table>
</asp:Content>
