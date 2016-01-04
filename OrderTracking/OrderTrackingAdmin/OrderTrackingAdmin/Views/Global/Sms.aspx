<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.GlobalViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Sms Provider
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.ActionLink("Global Administration", "/", "Global")%> > SMS Provider</h2>
<%
    using (Html.BeginForm("SaveSms", "Global", FormMethod.Post))
    {
%>
<input id="Id" name="Id" type="hidden" value="<%=Model.SmsCredential.Id.Value%>" /> 
    <table>
        <tr class="separator">
            <td colspan="3"></td>
        </tr>
        <tr>
            <td><strong>Username</strong></td><td><strong>Password</strong></td><td></td>
        </tr>        
        <tr>
            <td><%=Html.TextBox("Username", Model.SmsCredential.Username)%></td><td><%=Html.TextBox("Password", Model.SmsCredential.Password)%></td><td><input type="submit" value="Update Credentials" /></td>
        </tr>
    </table>
<%} %>
</asp:Content>