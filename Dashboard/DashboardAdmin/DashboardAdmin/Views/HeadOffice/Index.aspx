<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardAdminViewData.IndexViewData>" %>
<%@ Import Namespace="Dashboard.Dao.Domain"%>
<%@ Import Namespace="DashboardAdmin.Models"%>
<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Home</h2>
    <table>
        <tr class="separator">
           <td colspan="2"></td>
        </tr>
        <tr>
           <td colspan="2"><strong>Search</strong></td>
        </tr>
       
       <%
    using (Html.BeginForm("Site", "HeadOffice", FormMethod.Post))
    {
%>
        <tr>
            <td>Rameses Store Id</td><td></td>
        </tr>
        <tr>
            <td><%= Html.TextBox("Id")%></td><td><input type="submit" value="search" /></td>
        </tr>

<%
    }
%>        
    <tr class="separator">
           <td colspan="2"></td>
        </tr>
        <tr>
           <td colspan="2"><strong>Head Offices</strong></td>
        </tr>
               <%
           foreach (var headOffice in Model.HeadOffices)
           {%>
        <tr>
           <td colspan="2"><%= Html.ActionLink(headOffice.HeadOfficeName, "/Details/" + headOffice.Id.Value, "HeadOffice")%></td>
        </tr>
        <%
           }%> 
        <tr class="separator">
            <td colspan="2"></td>
        </tr> 

        </table>
       
</asp:Content>
