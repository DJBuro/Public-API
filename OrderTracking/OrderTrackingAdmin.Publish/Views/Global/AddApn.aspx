<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.GlobalViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add Apn
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.ActionLink("Global Administration", "/", "Global")%> > Add Apn</h2>
<%
    using (Html.BeginForm("CreateApn", "Global", FormMethod.Post))
    {
%>
    <table>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4"><strong>Provider</strong></td>
        </tr>        
        <tr>
            <td colspan="4"><%=Html.TextBox("Provider",null, new { @size = "30" })%></td>
        </tr>
        <tr>
            <td><strong>Name</strong></td><td><strong>Username</strong></td><td><strong>Password</strong></td><td></td>
        </tr>        
        <tr>
            <td><%=Html.TextBox("Name")%></td><td><%=Html.TextBox("Username")%></td><td><%=Html.TextBox("Password")%></td><td><input type="submit" value="Update Apn" /></td>
        </tr>
    </table>
<%} %>
</asp:Content>
