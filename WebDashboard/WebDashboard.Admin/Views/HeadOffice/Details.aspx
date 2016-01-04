<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, HeadOfficeDetails")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "HeadOffice")%> > <%= Model.HeadOffice.Name%></h2>
     <table>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4"><strong><%=Html.Resource("Master, HeadOfficeDetails")%></strong></td>
        </tr>
        <tr>
            <td><%= Html.ActionLink(Model.HeadOffice.Regions.Count + " " + Html.Resource("Master, Regions"), "/Region/" + Model.HeadOffice.Id.Value, "HeadOffice")%></td><td><%= Html.ActionLink(Model.HeadOffice.Sites.Count + " " + Html.Resource("Master, Sites"), "/Sites/" + Model.HeadOffice.Id.Value, "HeadOffice")%></td><td><%= Html.ActionLink(Model.HeadOffice.Users.Count + " " + Html.Resource("Master, Users"), "/Users/" + Model.HeadOffice.Id.Value, "HeadOffice")%></td><td></td>
        </tr>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4"><strong><%= Html.ActionLink("Dashboard", "/Dashboard/" + Model.HeadOffice.Id.Value, "HeadOffice")%></strong></td>
        </tr>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
          <%
              using (Html.BeginForm("UpdateHeadOffice", "HeadOffice", FormMethod.Post))
              {
%>
        <tr>
            <td colspan="4"><strong><%=Html.Resource("Master, HeadOffice")%></strong><input id="HeadOffice.Id" name="HeadOffice.Id" type="hidden" value="<%=Model.HeadOffice.Id.Value%>" /></td>
        </tr>
        <tr>
            <td colspan="4"><%=Html.Resource("Master, Name")%></td>
        </tr>
        <tr>
            <td colspan="4"><%=Html.TextBox("HeadOffice.Name", Model.HeadOffice.Name, new { @size = "30" })%></td>
        </tr>
        <tr>
            <td colspan="4"><%=Html.Resource("Master, Message")%></td>
        </tr>
        <tr>
            <td colspan="4"><%=Html.TextArea("HeadOffice.Message", Model.HeadOffice.Message, new { @cols = "60" })%></td>
        </tr>
        <tr>
            <td colspan="3"></td><td><input type="submit" value="<%=Html.Resource("Master, Update")%>" /></td>
        </tr>
        <%}
              using (Html.BeginForm("RemoveHeadOffice", "HeadOffice", FormMethod.Post))
              {%>
              <%=Html.Hidden("HeadOffice.Id", Model.HeadOffice.Id)%>
        <tr class="separator">
           <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4"><strong><%=Html.Resource("Master, Remove")%> <%=Model.HeadOffice.Name%></strong></td>
        </tr>
        <tr>
            <td colspan="3" class="error">All sites, regions, indictors and users will be deleted, <strong>this cannot be undone</strong></td><td><input type="submit" value="<%=Html.Resource("Master, Remove")%>" /></td>
        </tr>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <%
              }%>
     </table>

</asp:Content>
