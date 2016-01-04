<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%=WebDashboard.Web.ResourceHelper.GetString("LogOnTitle")%>
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%=WebDashboard.Web.ResourceHelper.GetString("LogOn").Replace(" ", "&nbsp;")%></h2>
    <p>
        <%=WebDashboard.Web.ResourceHelper.GetString("EnterEmailAndPassword")%>
    </p>
    <% using (Html.BeginForm()) { %>
        <div id="logon">
            <table>
                <tr>
                    <td>
                        <label for="username"><%=WebDashboard.Web.ResourceHelper.GetString("EmailAddress")%></label>
                    </td>
                    <td>
                        <%= Html.TextBox("username", null, new { @style = "width: 200px;" })%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="color:Red;">
                        <%= Html.ValidationMessage("username") %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="password"><%=WebDashboard.Web.ResourceHelper.GetString("Password")%></label>
                    </td>
                    <td>
                        <%= Html.Password("password", null, new { @style = "width: 200px;" })%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="color:Red;">
                        <%= Html.ValidationMessage("password") %>
                    </td>
                </tr>                
                <tr>
                    <td colspan="2" style="text-align:center"><input type="submit" value="<%=WebDashboard.Web.ResourceHelper.GetString("Submit")%>" /></td>
                </tr>
            </table>
        </div>

        <div id="cookiesDisabledDiv" name="cookiesDisabledDiv" style="display:none; text-align:center; padding-top:20px; color:red">
        Cookies appear to be disabled in your web browser.<br /><br />
        This website requires cookies to function correctly.<br /><br />
        Please ensure cookies are enabled in your web browser settings.</div>
        <script type="text/javascript">
            var cookieName = '_' + new Date().getTime();

            document.cookie = cookieName;
            cookieEnabled = (document.cookie.indexOf(cookieName) != -1) ? true : false;

            var cookiesDisabledDiv = document.getElementById('cookiesDisabledDiv');
            if (cookieEnabled) {
                cookiesDisabledDiv.style.display = 'none';
            }
            else {
                cookiesDisabledDiv.style.display = 'block';
            }
        </script>

    <% } %>
</asp:Content>
