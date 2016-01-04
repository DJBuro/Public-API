<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	permissions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2><%=Html.ActionLink("Users","Users","Home") %> > <%=Html.ActionLink(Model.User.EmailAddress,"User/" + Model.EditedUser.Id.Value,"Home") %> > Permissions</h2>

<table>
<%=Html.Hidden("User.Id", Model.EditedUser.Id)%>
<%
    foreach (var region in Model.Regions)
    {
%>
    <tr>
        <td colspan="3">
            <strong><%=region.Name %></strong>&nbsp;&nbsp;

<% 
            bool allStores = false;
            
            if (Model.UserRegions != null)
            {
                foreach (UserRegion userRegion in Model.UserRegions)
                {
                    if (userRegion.RegionId == region.Id)
                    {
                        allStores = true;        
                        break;
                    }
                }
            }
            
            if (allStores)
            {
%>           
            ALL STORES&nbsp;&nbsp;
            <%= Html.ActionLink("Select individual stores in this region", "DisableUserRegion/" + Model.EditedUser.Id + "/" + region.Id, "Home")%>
<% 
            }
            else
            {
%> 
            <%= Html.ActionLink("Allow all stores in this region", "EnableUserRegion/" + Model.EditedUser.Id + "/" + region.Id, "Home")%>
<% 
            }
%> 
        </td>
    </tr> 
<%
        foreach (Site site in region.RegionalSites)
        {
            var active = false;

            foreach (Permission permission in Model.EditedUser.UserPermissions)
            {
                if (permission.Site == site)
                {
                    active = true;
                    break;
                }
            }
%>
    <tr>
        <td align="right" class="<%= site.Enabled %>"></td>
        <td><%=site.Name%></td>
<%
        if (!allStores)
        {
%> 
        <td><%= active ? Html.ActionLink("Remove", "RemovePermission/" + Model.EditedUser.Id + "/" + site.SiteId, "Home") : Html.ActionLink("Add", "AddPermission/" + Model.EditedUser.Id + "/" + site.SiteId, "Home")%></td>
<%
        }
        else
	    {
%>
        <td></td>
<%    
	    }
%>

    </tr>   
    <%
        }//end foreach site
    %>  
    <tr class="separator">
       <td colspan="3"></td>
    </tr>
    <%
    }//end foreach region
    %>
    </table>
</asp:Content>
