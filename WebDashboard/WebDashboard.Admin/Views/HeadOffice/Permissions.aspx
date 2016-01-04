<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Permissions
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <h2><%= Html.ActionLink("Home", "/", "HeadOffice")%> > <%= Html.ActionLink(Model.HeadOffice.Name, "/Details/" + Model.HeadOffice.Id, "HeadOffice")%>  > <%= Html.ActionLink("Users", "/Users/" + Model.HeadOffice.Id, "HeadOffice")%> > <%= Html.ActionLink("User", "/User/" + Model.User.Id, "HeadOffice")%> > Permissions</h2>
<table>
<%=Html.Hidden("User.Id", Model.User.Id)%>
    <tr class="separator">
       <td colspan="4"></td>
    </tr> 
<%
    foreach (var region in Model.Regions)
    {%>
    <tr>
       <td colspan="2"><strong><%=region.Name %></strong></td><td colspan="2">Rameses Site Id</td>
    </tr> 
    <%
        foreach (Site site in region.RegionalSites)
        {
            var active = false;
            
            foreach (Permission permission in Model.User.UserPermissions)
            {
                if (permission.Site != site) continue;
                active = true;
                break;
            }
    %>
    <tr>
        <td align="right" class="<%= active %>"></td><td><%=site.Name%></td><td><%=site.SiteId%></td><td><%= active ? Html.ActionLink(Html.Resource("Master, Remove"), "RemovePermission/" + Model.User.Id + "/" + site.SiteId, "HeadOffice") : Html.ActionLink(Html.Resource("Master, Add"), "AddPermission/" + Model.User.Id + "/" + site.SiteId, "HeadOffice")%></td>
    </tr>   
    <%
        }//end foreach site
    %>  
    <tr class="separator">
       <td colspan="4"></td>
    </tr>
    <%
    }//end foreach region
    %>
    </table>
</asp:Content>
