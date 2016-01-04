<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        <strong><%= Html.Encode(Page.User.Identity.Name) %></strong>
        [ <%= Html.ActionLink("Log Off", "LogOff", "Account") %> ]
<%
    }else{
%>
<%} %>