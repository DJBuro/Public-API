<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Dashboard.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Net"%>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
Log On to the Dashboard
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="siteMessage">
        <strong>Andromeda Dashboard - Logon - <%= this.ViewData["ipAddress"]%></strong>
    </div>

 <div id="accountContent">
    <p>
       <strong>Please enter your username and password.</strong>        
    </p>
    <%= Html.ValidationSummary("Login was unsuccessful") %>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend>Account Information</legend>
                <p>
                    <label for="username">Username:</label>
                    <%= Html.TextBox("username") %>
                    
                </p>
                <p>
                    <label for="password">Password:</label>
                    <%= Html.Password("password") %>
                    
                </p>
                <p>
                    <%= Html.CheckBox("rememberMe") %> <label class="inline" for="rememberMe">Remember me?</label>
                </p>
                <p>
                    <input type="submit" value="Log On" />
                </p>
            </fieldset>
        </div>
    <% } %>
    
   </div>
</asp:Content>
