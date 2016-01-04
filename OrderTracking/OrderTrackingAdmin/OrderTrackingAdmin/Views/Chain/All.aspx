<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.ClientViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	All Chains
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Chains</h2>
    <%= Html.ActionLink(Html.Resource("Master, AddNewChain"), "Add", "Chain")%>
    <table>
        <tr class="separator">
            <td colspan="2"></td>
        </tr>
        <tr>
            <td>
                <strong><%=Html.Resource("Master, Name")%></strong>
            </td>
            <td>
                <strong><%=Html.Resource("Master, WebsiteTemplate")%></strong>
            </td>
            <td>
                <strong><%=Html.Resource("Master, WebKey")%></strong>
            </td>
        </tr>
    <%
    foreach (Client client in Model.Clients)
    {%>
         <tr>
            <td>
                <%= Html.ActionLink(client.Name, "/Edit/" + client.Id.ToString(), "Chain")%>
            </td>
            <td>
                <%= client.WebsiteTemplateName %>
            </td>
            <td>
                <%= client.WebKey %>
            </td>
        </tr> 
        <%
    }%>
    </table>

</asp:Content>
