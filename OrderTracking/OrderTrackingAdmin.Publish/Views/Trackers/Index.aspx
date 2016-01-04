<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.TrackerViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, Trackers") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%=Html.Resource("Master, Trackers") %></h2>
    <table>
        <tr class="separator">
           <td colspan="2"></td>
        </tr>
        <tr>
           <td><strong><%= Html.ActionLink("View All Trackers", "/All/","Trackers")%></strong></td><td><strong><%= Html.ActionLink("Add New Tracker", "Add", "Trackers")%></strong></td>
        </tr>
        <tr class="separator">
           <td colspan="2"></td>
        </tr>
        <tr>
           <td colspan="2"><strong><%=Html.Resource("Master, Search")%></strong></td>
        </tr>
<%
    using (Html.BeginForm("FindByStore", "Trackers", FormMethod.Post))
    {
%>
        <tr>
            <td><%=Html.Resource("Master, RamesesStoreId")%></td><td></td>
        </tr>
        <tr>
            <td><%= Html.TextBox("id")%></td><td><input type="submit" value="<%=Html.Resource("Master, Search")%>" /></td>
        </tr>
        <tr class="separator">
            <td colspan="2"></td>
        </tr> 
<%
    }
    using (Html.BeginForm("FindByPhone", "Trackers", FormMethod.Post))
    {
%>
        <tr>
            <td>Tracker Phone Number</td><td></td>
        </tr>
        <tr>
            <td><%= Html.TextBox("id")%></td><td><input type="submit" value="<%=Html.Resource("Master, Search")%>" /></td>
        </tr>
        <tr class="separator">
            <td colspan="2"></td>
        </tr> 
<%
    }
    using (Html.BeginForm("FindByName", "Trackers", FormMethod.Post))
    {
%>        
        <tr>
            <td>Tracker Name</td><td></td>
        </tr>
        <tr>
            <td><%= Html.TextBox("id")%></td><td><input type="submit" value="<%=Html.Resource("Master, Search")%>" /></td>
        </tr>
        <tr class="separator">
            <td colspan="2"></td>
        </tr> 
<%
    }
 %>
        
        </table>
</asp:Content>
