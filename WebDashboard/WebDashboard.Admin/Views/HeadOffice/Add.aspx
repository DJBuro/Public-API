<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, AddNewHeadOffice")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "HeadOffice")%> > <%=Html.Resource("Master, AddNewHeadOffice")%></h2>
  <%
      using (Html.BeginForm("AddHeadOffice", "HeadOffice", FormMethod.Post))
      {
%>
<table>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4"><strong><%=Html.Resource("Master, HeadOffice")%></strong></td>
        </tr>
        <tr>
            <td colspan="1"><%=Html.Resource("Master, Name")%></td><td colspan="3"><%=Html.Resource("Master, RamesesStoreId")%></td>
        </tr>
        <tr>
            <td colspan="1"><%=Html.TextBox("HeadOffice.Name", "", new {@size = "30"})%></td><td colspan="3"><%=Html.TextBox("HeadOffice.Id", "", new {@size = "10"})%></td>
        </tr>
        <tr>
            <td colspan="4"><%=Html.Resource("Master, Message")%></td>
        </tr>
        <tr>
            <td colspan="4"><%=Html.TextBox("HeadOffice.Message", "", new {@size = "80"})%></td>
        </tr>
        <tr>
            <td colspan="3"></td><td><input type="submit" value="<%=Html.Resource("Master, AddNewHeadOffice")%>" /></td>
        </tr>
</table>
<%
      }%>
</asp:Content>
