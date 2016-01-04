<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%=Html.ActionLink("Users","Users","Home") %> > <%=Model.EditedUser.EmailAddress%></h2>
    
<%
        using (Html.BeginForm("UpdateUser", "Home", FormMethod.Post, new { @id = "form1" }))
        {
%> 
<table>
<%=Html.Hidden("Id", Model.EditedUser.Id)%>
    <tr>
        <td><strong>Edit User</strong></td>
        <td class="error"><%= Model.ErrorMessage ?? "" %></td>
    </tr> 
    <tr>
        <td><%=Html.CheckBox("Active", Model.EditedUser.Active)%>&nbsp;Active</td>
        <td></td>
    </tr>
    <tr>
        <td>Email Address</td>
        <td>Security Access</td>
    </tr>
    <tr>
        <td><%=Html.TextBox("EmailAddress", Model.EditedUser.EmailAddress, new { @size = "30", @class = "{validate:{required:true,email:true,messages:{required:'Please enter an email address', email:'Please enter a valid email address'}}}" })%></td>
        <td><%=Html.CheckBox("StoreUser", Model.EditedUser.StoreUser)%>&nbsp;Store Dashboard</td>
    </tr>
    <tr>
        <td>Password</td>
        <td><%=Html.CheckBox("IsExecutiveDashboardUser", Model.EditedUser.IsExecutiveDashboardUser)%>&nbsp;Executive Dashboard</td>
    </tr>
    <tr>
        <td><%=Html.TextBox("Password", Model.EditedUser.Password, new { @size = "30", @class = "{validate:{required:true,minlength:5,messages:{required:'Please enter a password'}}}" })%></td>
        <td><%=Html.CheckBox("IsAdministrator", Model.EditedUser.IsAdministrator)%>&nbsp;Administration</td>
    </tr>
    <tr>
        <td colspan="2"><input type="submit" value="Update" /></td>
    </tr>
    
<%
        }

        if (!Model.EditedUser.Active)
        {
            using (Html.BeginForm("RemoveUser", "Home", FormMethod.Post))
            {
%>
                   <%=Html.Hidden("Id", Model.EditedUser.Id)%>
     <tr>
         <td colspan="2"><input type="submit" value="Delete User" /></td>
     </tr>                  
<% 
            }
        }
%>
    <tr class="separator">
       <td colspan="2"></td>
    </tr> 
<%
        // Does the users access level give the user permissions?
        if (Model.EditedUser.StoreUser || Model.EditedUser.IsExecutiveDashboardUser)
        {
            // User is allowed permissions so provide a button to edit them
%>
    <tr>
        <td>
            <strong>User Permissions</strong>
        </td>
        <td>
            <%= Html.ActionLink("Edit Permissions", "Permissions/" + Model.EditedUser.Id.Value, "Home")%>
        </td>
    </tr>
<%
            // Display the current permissions
            if (Model.EditedUser.UserPermissions.Count == 0 && (Model.UserRegions == null || Model.UserRegions.Count == 0))
            {
%>
    <tr>
        <td colspan="2">User currently has no permissions</td>
    </tr>
<%
            }
            else
            {              
                // Order stores by region
                foreach (Region region in Model.Regions)
                {
                    bool allStores = false;
            
                    // Does this user have permission to veiw specific regions?
                    if (Model.UserRegions != null)
                    {
                        // Does the user have permission to see all the stores in this region?
                        foreach (UserRegion userRegion in Model.UserRegions)
                        {
                            if (userRegion.RegionId == region.Id)
                            {
                                // User has permission to see all the stores in this region
                                allStores = true;        
                                break;
                            }
                        }
                    }

                    // Does the user have permission to see all the stores in this region?
                    if (allStores)
                    {
%>
    <tr>
        <td colspan="2">
            <strong><%= region.Name%></strong>
        </td>
    </tr>
<%
                        // Display each site in the region
                        foreach (Site site in region.RegionalSites)
                        {
%>
    <tr>
        <td colspan="2" class="<%= site.Enabled %>">
            <%= site.Name%>
        </td>
    </tr>
<%  
                        }
                    }
                    else
                    {
                        bool regionHeadingGenerated = false;

                        // Only show the sites in this region that the user has permission to view
                        foreach (Site site in region.RegionalSites)
                        {
                            foreach (Permission permission in Model.EditedUser.UserPermissions)
                            {
                                // Does the user have permission to view this site?
                                if (permission.Site.Id == site.Id)
                                {
                                    // Yes - user can view this site.
                                    // Have we displayed a region header for this region?
                                    // (we do it this way so that the region header is only displayed if the user has permission
                                    // to view one or more store in this region - otherwise empty region groups are displayed)
                                    if (!regionHeadingGenerated)
                                    {
                                        regionHeadingGenerated = true;
%>
    <tr>
        <td colspan="2">
            <strong><%= region.Name%></strong>
        </td>
    </tr>
<%
                            }
%>
    <tr>
        <td colspan="2" class="<%= site.Enabled %>">
            <%= site.Name%>
        </td>
    </tr>
<%  
                                }
                            }
                        }
                    }
                }
            }
        }
        else if (Model.EditedUser.IsAdministrator)
        {
            // User only has admin access only
%>     
    <tr>
        <td>
            <strong>User Permissions</strong>
        </td>
    </tr>
    <tr>
        <td>This user has administrative access only</td>
    </tr>
<%
        }
        else
        {
            // User is not allowed permissions so no button required
%>     
    <tr>
        <td>
            <strong>User Permissions</strong>
        </td>
    </tr>
    <tr>
        <td>This user has no access to site data</td>
    </tr>
<%
        }
%>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HelpContent" runat="server">
                        <li>"Store Dashboard" allows access to the realtime store dashboard</li>
                        <li>"Executive Dashboard" allows access to the company wide statistics</li>
                        <li>"Administration" allows access to this admin system</li>
                        <li>Only users that are not active can be deleted</li>
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