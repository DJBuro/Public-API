<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Web.Models"%>
<%@ Import Namespace="WebDashboard.Web"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    site
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2><%=Html.ActionLink("Sites","Sites","Home") %> > <%= Model.Site.Name %></h2>
<%
    using (Html.BeginForm("UpdateSite", "Home", FormMethod.Post, new { @id = "form1" }))
    {
%> 
<%=Html.Hidden("Site.Id", Model.Site.Id)%>
<%=Html.Hidden("Site.Key", Model.Site.Key)%>
<%=Html.Hidden("Site.SiteId", Model.Site.SiteId)%>
<%=Html.Hidden("Site.LastUpdated", Model.Site.LastUpdated)%>
<%=Html.Hidden("Site.HeadOffice.Id", Model.Site.HeadOffice.Id)%>
<table>
    <tr>
        <td>Rameses Site Id</td>
        <td>Name</td>
        <td><%=Html.CheckBox("Site.Comp", Model.Site.Comp)%> COMP</td>
    </tr>
    <tr>
        <td><%=Model.Site.SiteId%></td>
        <td><%=Html.TextBox("Site.Name", Model.Site.Name, new { @class = "{validate:{required:true,minlength:3,messages:{required:'Please enter a Name'}}}" })%></td>
        <td><%=Html.CheckBox("Site.Enabled", Model.Site.Enabled)%> Active</td>
    </tr>
    <tr>
        <td>Site Type</td>
        <td>IP Address</td>
        <td>Last Data Update</td>
    </tr>
    <tr>
        <td><%=Html.DropDownList("Site.SiteType.Id", Model.SiteTypeListItems)%></td>
        <td><%=Html.TextBox("Site.IPAddress", Model.Site.IPAddress, new { @class = "{validate:{required:true,minlength:7,messages:{required:'Please enter an IP Address'}}}" })%></td>
        <td class="<%= (Model.Site.LastUpdated.Value.Day == DateTime.Now.Day && Model.Site.LastUpdated.Value.Month == DateTime.Now.Month) ? "" : "Error" %>"><%= String.Format("{0:G}", Model.Site.LastUpdated)%></td>
    </tr>
    <tr>
        <td colspan="3">Region</td>
    </tr>
    <tr>
        <td colspan="2"><%=Html.DropDownList("Site.Region.Id", Model.RegionListItems)%></td>
        <td><input type="submit" value="Update" /></td>
    </tr>
    <%
    }//end update siteRegion

    if (Model.Site.Enabled && Model.Site.SiteType.Id ==1)
    {
%> 
    <tr class="separator">
       <td colspan="3"></td>
    </tr> 
    <tr>
        <td colspan="3"><a href="/flex2/index.html#<%=Obfuscator.encryptString(Model.Site.Key.ToString())%>" target="_blank">View <%= Model.Site.Name%> Dashboard</a></td>
    </tr>
    
    <%}//end site enabled %>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HelpContent" runat="server">
                        <li>Only 'Active' Stores can have a dashboard</li>
                        <li>IP Address</li>
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