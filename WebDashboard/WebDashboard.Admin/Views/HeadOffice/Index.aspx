<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>
<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
   <%=Html.Resource("Master, DashboardAdministration")%> 
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%=Html.Resource("Master, Home")%></h2>
    <table>
        <tr class="separator">
           <td colspan="2"></td>
        </tr>
        <tr>
           <td colspan="2"><strong><%=Html.Resource("Master, Search")%></strong></td>
        </tr>
       
       <%
    using (Html.BeginForm("Search", "HeadOffice", FormMethod.Post))
    {
%>
        <tr>
            <td><%=Html.Resource("Master, RamesesStoreId")%></td><td></td>
        </tr>
        <tr>
            <td><%= Html.TextBox("Id")%></td><td><input type="submit" value="<%=Html.Resource("Master, Search")%>" /></td>
        </tr>
<%
    }
%>        
    <tr class="separator">
           <td colspan="2"></td>
        </tr>
        <tr>
           <td colspan="2"><strong><%=Html.Resource("Master, HeadOffices")%></strong></td>
        </tr>
               <%
           foreach (var headOffice in Model.HeadOffices)
           {%>
        <tr>
           <td colspan="2"><%= Html.ActionLink(headOffice.Name, "/Details/" + headOffice.Id.Value, "HeadOffice")%></td>
        </tr>
        <%
           }%> 
        <tr class="separator">
            <td colspan="2"></td>
        </tr> 

        </table>
       
</asp:Content>