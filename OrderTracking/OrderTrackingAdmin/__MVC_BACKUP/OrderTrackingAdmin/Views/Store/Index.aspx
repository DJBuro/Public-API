<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.IndexViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Andro OrderTracking Administration Area
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class=""><% if (Model.ErrorMessage != null)
                 {%>  <%= Model.ErrorMessage%> <%} %></div>

    <h2><%=Html.Resource("Master, Home")%></h2>
    
    <table>
        <tr class="separator">
           <td colspan="2"></td>
        </tr>
        <tr>
           <td colspan="2"><strong><%= Html.ActionLink(Html.Resource("Master, ViewAllStores"), "/All/","Store")%></strong></td>
        </tr>
        <tr class="separator">
           <td colspan="2"></td>
        </tr>
        <tr>
           <td colspan="2"><strong><%=Html.Resource("Master, Search")%></strong></td>
        </tr>
<%
    using (Html.BeginForm("FindById", "Store", FormMethod.Post))
    {
%>
        <tr>
            <td><%=Html.Resource("Master, RamesesStoreId")%></td><td></td>
        </tr>
        <tr>
            <td><%= Html.TextBox("Store.ExternalStoreId")%></td><td><input type="submit" value="<%=Html.Resource("Master, Search")%>" /></td>
        </tr>
        <tr class="separator">
            <td colspan="2"></td>
        </tr> 
<%
    }
    using (Html.BeginForm("FindByName", "Store", FormMethod.Post))
    {
%>        
        <tr>
            <td><%=Html.Resource("Master, StoreName")%></td><td></td>
        </tr>
        <tr>
            <td><%= Html.TextBox("Store.Name")%></td><td><input type="submit" value="<%=Html.Resource("Master, Search")%>" /></td>
        </tr>
        <tr class="separator">
            <td colspan="2"></td>
        </tr> 
<%
    }
 %>
 
  <tr class="separator">
       <td colspan="3"></td>
    </tr> 
        <tr>
           <td colspan="2"><strong><%= Html.ActionLink(Html.Resource("Master, GlobalLogs"), "/WeeksGlobalLogs/", "Store")%></strong></td>
        </tr>
    </table>


</asp:Content>
