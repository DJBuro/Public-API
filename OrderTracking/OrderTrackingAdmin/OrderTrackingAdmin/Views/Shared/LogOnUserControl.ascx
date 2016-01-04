<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%
    if (Request.IsAuthenticated) {
%>
        <strong><%= Html.Encode(Page.User.Identity.Name) %></strong>
        [ <%= Html.ActionLink(Html.Resource("Master, LogOff"), "LogOff", "Account") %> ]
<%
    }
%>
