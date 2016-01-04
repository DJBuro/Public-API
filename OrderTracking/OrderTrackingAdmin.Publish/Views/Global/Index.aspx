<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.GlobalViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Global Administration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Global Administration</h2>
    <table>
        <tr class="separator">
           <td colspan="2"></td>
        </tr>
        <tr>
           <td colspan="2"><strong><%= Model.ViewString %></strong></td>
        </tr>
        <tr class="separator">
           <td colspan="2"></td>
        </tr>
        <tr>
           <td><strong>APN Provider</strong></td><td><%= Html.ActionLink("Add New APN", "AddApn", "Global")%></td>
        </tr>
        <%
            foreach (var apn in Model.Apns)
            {%>
        <tr>
           <td><%= Html.ActionLink(apn.Provider, "Apn/" + apn.Id, "Global")%></td><td></td>
        </tr>   
            
            <%    
            } %>
        <tr class="separator">
           <td colspan="2"></td>
        </tr>     
        <tr>
           <td><strong><%= Html.ActionLink("SMS Provider", "SmsProvider", "Global")%></strong></td><td></td>
        </tr>
        <tr class="separator">
           <td colspan="2"></td>
        </tr>        
        <tr>
           <td><strong>Tracker Types</strong></td><td><a href="#">Add Type of Tracker</a></td>
        </tr>
        <tr>
           <td><a href="#">GT30</a></td><td>todo: commands/Setup command types</td>
        </tr>
          <tr class="separator">
               <td colspan="2"></td>
        </tr> 
        <tr>
           <td colspan="2"><strong><%= Html.ActionLink(Html.Resource("Master, GlobalLogs"), "/WeeksGlobalLogs/", "Global")%></strong></td>
        </tr>
    </table>
</asp:Content>
