<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Users</h2>
<%
    using (Html.BeginForm("CreateUser", "Home", FormMethod.Post, new { @id = "form1" }))
    {
%>
<%=Html.Hidden("User.Id", Model.User.Id)%>
<table>
    <tr>
        <td><strong>Add New User</strong></td>
        <td class="Error"><%= Model.ErrorMessage ?? "" %></td>
    </tr> 
    <tr>
        <td><%=Html.CheckBox("User.Active",true)%>&nbsp;Active</td>
        <td></td>
    </tr>
    <tr>
        <td>Email Address</td>
        <td>Security Access</td>
    </tr>
    <tr>
        <td><%=Html.TextBox("User.EmailAddress", "", new { @size = "30", @class = "{validate:{required:true,email:true,messages:{required:'Please enter an email address', email:'Please enter a valid email address'}}}" })%></td>        
        <td><%=Html.CheckBox("User.StoreUser", true)%>&nbsp;Store Dashboard</td>
    </tr>
    <tr>
        <td>Password</td>
        <td><%=Html.CheckBox("User.IsExecutiveDashboardUser", false)%>&nbsp;Executive Dashboard</td>
    </tr>
    <tr>
        <td><%=Html.TextBox("User.Password", "", new { @size = "30", @class = "{validate:{required:true,minlength:5,messages:{required:'Please enter a password'}}}" })%></td>
        <td><%=Html.CheckBox("User.IsAdministrator", false)%>&nbsp;Administration</td>
    </tr>
    <tr>
        <td colspan="2"><input type="submit" value="Add User" /></td>
    </tr>
</table>
<table>
    <tr>
        <td><strong><%= Model.User.HeadOffice.Name %> Users</strong></td>
    </tr> 
    <tr>
        <td></td>
        <td colspan="3" style="text-align:center"><strong>Security Access</strong></td>
    </tr>
    <tr>
        <td><strong>Username/Email</strong></td>
        <td><strong>Store</strong></td>
        <td><strong>Executive</strong></td>
        <td><strong>Admin</strong></td>
    </tr>
    <%
        foreach (var user in Model.Users)
        {%>
        
     <tr>
        <td class="<%= user.Active %>"> <%= Html.ActionLink(user.EmailAddress,"User/" + user.Id.Value,"Home") %></td>
        <td><%= user.StoreUser ? "yes" : "" %></td>
        <td><%= user.IsExecutiveDashboardUser ? "yes" : "" %></td>
        <td><%= user.IsAdministrator ? "yes" : "" %></td>
     </tr>
        
        <%    
        } %>
    
    
</table>
<%
    }//end form%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HelpContent" runat="server">
                        <li>"Store Dashboard" allows access to the realtime store dashboard</li>
                        <li>"Executive Dashboard" allows access to the company wide statistics</li>
                        <li>"Administration" allows access to this admin system</li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ValidatorContent" runat="server">
<script type="text/javascript">
    $(document).ready(function() {

        var container = $('div.container');

        var validator = $("#form1").validate({
            errorContainer: container,
            errorLabelContainer: $("ul", container),
            wrapper: 'li',
            meta: "validate"
        });
    });
</script>
</asp:Content>