<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%=WebDashboard.Web.ResourceHelper.GetString("Welcome")%></h2>
    <p>
        <%=WebDashboard.Web.ResourceHelper.GetString("Please")%>  <% Html.RenderPartial("LogOnUserControl"); %>
    </p>
</asp:Content>
