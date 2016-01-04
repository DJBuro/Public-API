<%@ Import Namespace="WebDashboard.Web"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<span style="padding-right:10px;">

<%= Html.ImageActionLink("http://dashboard.androtechnology.co.uk/dashboard/Images/UKUSFlagSmall.png", "English", "ChangeCulture", "Account", new { lang = "en", returnUrl = this.Request.RawUrl }, null, null)%>

<%= Html.ImageActionLink("http://dashboard.androtechnology.co.uk/dashboard/Images/FrenchFlagSmall.png", "French", "ChangeCulture", "Account", new { lang = "fr-FR", returnUrl = this.Request.RawUrl }, null, null)%>

<%= Html.ImageActionLink("http://dashboard.androtechnology.co.uk/dashboard/Images/RussianFlagSmall.png", "Russian", "ChangeCulture", "Account", new { lang = "ru-RU", returnUrl = this.Request.RawUrl }, null, null)%>

<%= Html.ImageActionLink("http://dashboard.androtechnology.co.uk/dashboard/Images/TurkishFlagSmall.png", "Turkish", "ChangeCulture", "Account", new { lang = "tr-TR", returnUrl = this.Request.RawUrl }, null, null)%>

<%-- Html.ImageActionLink("http://dashboard.androtechnology.co.uk/dashboard/Images/AzerbaijanFlagSmall.png", "Azerbaijan", "ChangeCulture", "Account", new { lang = "az", returnUrl = this.Request.RawUrl }, null, null)--%>

</span>