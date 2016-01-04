<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
       <%-- <%= Html.Encode(Page.User.Identity.Name) %>--%>
        <%= Html.ActionLink("Log Off", "LogOff", "Account") %>
<%
    }
    else {
%> 
        <%= Html.ActionLink("Log On", "LogOn", "Account") %>
<%
    }
%>
