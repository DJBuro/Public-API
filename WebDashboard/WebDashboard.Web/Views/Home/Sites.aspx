<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>
<%@ Import Namespace="WebDashboard.Web"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    sites
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Sites</h2>
    
    <table>
<%
    
    if (Model.User.IsAdministrator)
    {
        // Order stores by region
        foreach (Region region in Model.Regions)
        {
%>
        <tr>
            <td style="padding: 5px 0 0 5px;">
                <strong><%= region.Name%></strong>
            </td>
            <td>
                <strong>Last Data Update</strong>
            </td>
            <td style="padding: 5px 0 0 5px;">
                <strong>Comp</strong>
            </td>
        </tr>
<%
            // Display each site in the region
            foreach (Site site in region.RegionalSites)
            {
%>
        <tr>
            <td class="<%= site.Enabled %>" style="width:350px; padding: 5px 0 0 25px;">
                <%= Html.ActionLink(site.Name, "Site/" + site.Id, "Home") %>
            </td>
            <td class="<%= (site.LastUpdated.Value.Day == DateTime.Now.Day && site.LastUpdated.Value.Month == DateTime.Now.Month)  ? "" : "Error" %>">
                <%= String.Format("{0:G}", site.LastUpdated)%>
            </td>
            <td class="<%= site.Comp ? "Tick" : "" %>" style="width:18px;">&nbsp;</td>
        </tr>
<%    
            }
        }
    }
    // All other users can view dashboards but only for stores that they have been granted permission
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
            <td style="padding: 5px 0 0 5px;">
                <strong><%= region.Name%></strong>
            </td>
            <td>
                <strong>Last Data Update</strong>
            </td>
            <td style="padding: 5px 0 0 5px;">
                <strong>Comp</strong>
            </td>
        </tr>
<%
                // Display each site in the region
                foreach (Site site in region.RegionalSites)
                {
%>
        <tr>
            <td class="<%= site.Enabled %>" style="width:350px; padding: 5px 0 0 25px;">
                <a href="/flex2/index.html#<%=Obfuscator.encryptString(site.Key.ToString())%>" target="_blank"><%= site.Name%></a>
            </td>
            <td class="<%= (site.LastUpdated.Value.Day == DateTime.Now.Day && site.LastUpdated.Value.Month == DateTime.Now.Month)  ? "" : "Error" %>">
                <%= String.Format("{0:G}", site.LastUpdated)%>
            </td>
            <td class="<%= site.Comp ? "Tick" : "" %>" style="width:18px;">&nbsp;</td>
        </tr>
<%              
                }
            }
            else
            {
                bool regionHeadingGenerated = false;
                var active = false;

                // Only show the sites in this region that the user has permission to view
                foreach (Site site in region.RegionalSites)
                {
                    foreach (Permission permission in Model.User.UserPermissions)
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
            <td style="padding: 5px 0 0 5px;">
                <strong><%= region.Name%></strong>
            </td>
            <td>
                <strong>Last Data Update</strong>
            </td>
            <td style="padding: 5px 0 0 5px;">
                <strong>Comp</strong>
            </td>
        </tr>
<%
                            }
%>
        <tr>
            <td class="<%= site.Enabled %>" style="width:350px; padding: 5px 0 0 25px;">
                <a href="/flex2/index.html#<%=Obfuscator.encryptString(permission.Site.Key.ToString())%>" target="_blank"><%= site.Name%></a>
            </td>
            <td class="<%= (site.LastUpdated.Value.Day == DateTime.Now.Day && site.LastUpdated.Value.Month == DateTime.Now.Month)  ? "" : "Error" %>">
                <%= String.Format("{0:G}", site.LastUpdated)%>
            </td>
            <td class="<%= site.Comp ? "Tick" : "" %>" style="width:18px;">&nbsp;</td>
        </tr>
<%  
                        }
                    }
                }
            }
        }
    }
%>
    
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HelpContent" runat="server">
                        <li><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/active.gif" /> Site Active</li>
                        <li><img src="http://dashboard.androtechnology.co.uk/dashboard/Images/inactive.gif" /> Site In-active</li>
</asp:Content>