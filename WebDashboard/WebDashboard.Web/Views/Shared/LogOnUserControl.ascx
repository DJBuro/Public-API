<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        <%= Html.Encode(Page.User.Identity.Name) %>
        <a href='<%= Url.Action("LogOff", "Account") %>'><%= WebDashboard.Web.ResourceHelper.GetString("LogOff").Replace(" ", "&nbsp;") %></a>
        <%--<%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("LogOff").Replace(" ", "&nbsp;"), "LogOff", "Account") %>--%>
<%
    }
    else {
%> 
        <a href='<%= Url.Action("LogOn", "Account") %>'><%= WebDashboard.Web.ResourceHelper.GetString("LogOn").Replace(" ", "&nbsp;") %></a>
        <%--<%= Html.ActionLink(WebDashboard.Web.ResourceHelper.GetString("LogOn").Replace(" ", "&nbsp;"), "LogOn", "Account")%>--%>
<%
    }
%>
