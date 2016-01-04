<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.ClientViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add New Chain
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%
    using (Html.BeginForm("Add", "Chain", FormMethod.Post))
    {
%>
    <table>
        <tr class="separator">
            <td colspan="2"></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, ChainName")%></td>
            <td><%=Html.TextBox("Name", null, new { style = "width:450px;" })%></td>
        </tr>
        <tr>
            <td><%=Html.Resource("Master, WebsiteTemplate")%></td>
            <td><%=Html.ListBox("WebsiteTemplateName", Model.WebsiteTemplates, new { style = "width:450px;" })%></td>
        </tr>
    </table>
    
    <input type="submit" value="<%=Html.Resource("Master, Accept")%>" />
   
<%
    }
%>

<%= Html.ActionLink(Html.Resource("Master, Cancel"), "All", "Chain")%>

</asp:Content>
