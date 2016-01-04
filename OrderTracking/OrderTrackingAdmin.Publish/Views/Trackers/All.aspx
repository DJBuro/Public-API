<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.TrackerViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View All Trackers
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%=Html.ActionLink(Html.Resource("Master, Trackers"), "/", "Trackers")%> > View All Trackers</h2>
    <table>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td><strong>Store</strong></td><td><strong>Tracker Name</strong></td><td><strong>Phone Number</strong></td><td><strong>Active</strong></td>
        </tr>
<%

    long? stId = 0;
    bool vis = true;

    //make sure we have a store that has trackers assigned
    if (Model.Trackers.Count > 0)
    {
        stId = Model.Trackers[0].Store.Id;

    }

    foreach (var tracker in Model.Trackers)
    {
        
        
        if(stId != tracker.Store.Id)
            {
                stId = tracker.Store.Id;
                vis = true;
            %>
        <tr class="separator">
            <td colspan="4"></td>
        </tr>
           <%
            
            }// end if
        
        %>        
        <tr>
            <td><%= vis ? Html.ActionLink(tracker.Store.Name, "/FindByExternalId/" + tracker.Store.ExternalStoreId, "Store").ToHtmlString(): ""%></td><td><%= Html.ActionLink(tracker.Name, "/Tracker/" + tracker.Name, "Trackers")%></td><td><%= tracker.PhoneNumber%></td><td class="<%= tracker.Active%>"></td>
        </tr>
   
        <%
        vis = false;
    }//end foreach
    
     %>
</table>

</asp:Content>
