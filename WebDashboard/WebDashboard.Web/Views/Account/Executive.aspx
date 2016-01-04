<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Executive
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h2>Log On</h2>
    <p>
        Please enter your email address and password. 
    </p>
    <% using (Html.BeginForm()) { %>
        <div id="logon">
                <p>
                    <label for="username">Email Address:</label>
                    <%= Html.TextBox("username") %>
                    <%= Html.ValidationMessage("username") %>
                </p>
                <p>
                    <label for="password">Password:</label>
                    <%= Html.Password("password") %>
                    <%= Html.ValidationMessage("password") %>
                </p>
                <p>
                    <input type="submit" value="Log On" />
                </p>
        </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HelpContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ValidatorContent" runat="server">
</asp:Content>
