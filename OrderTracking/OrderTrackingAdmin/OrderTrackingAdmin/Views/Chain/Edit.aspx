<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.ClientViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Chain
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Chain Details</h2>
    
    <table>
        <tr>
            <td style="font-weight:bold; width:150px;"><%=Html.Resource("Master, Name")%></td>
            <td><%= Model.Client.Name %></td>
        </tr>
        <tr>
            <td style="font-weight:bold; width:150px;"><%=Html.Resource("Master, WebKey")%></td>
            <td><%= Model.Client.WebKey%></td>
        </tr>
        <tr>
            <td style="font-weight:bold; width:150px;"><%=Html.Resource("Master, URL")%></td>
            <% 
                if (Model.Url == null || Model.Url.Length == 0)
                {
            %>
            <td><%=Html.Resource("Master, NoWebSite")%></td>
            <%
                }
                else
                {
                    string link = "<a href=\"" + Model.Url + "\" target=\"_blank\">" + Model.Url + "</a>"; 
            %>
            <td><%= link%></td>
            <%
                } 
            %>
        </tr>
    </table>
    <br />
    <h2>Edit Chain</h2>
<%
    using (Html.BeginForm("Edit", "Chain", FormMethod.Post))
    {
%>
        <h3><%=Html.Resource("Master, WebsiteTemplate")%></h3>
        <p><%=Html.Resource("Master, WebsiteTemplateWarning")%></p>
        <%=Html.ListBox("WebsiteTemplateName", Model.WebsiteTemplates, new { style = "width:500px; margin-top:5px;" })%>
        
        <h3 style="margin-top:10px;">Stores in chain (tick to remove)</h3>
<%
        foreach (Store store in Model.Stores)
        {%>
            <%= Html.CheckBox("store_" + store.Id.ToString(), false)%>
            <%= store.ExternalStoreId %> / <%= store.Name %>
            <br />
        <%
        }%>
    <br />
        <input type="submit" value="<%=Html.Resource("Master, ApplyChanges")%>" />
        
        <div style="margin-top:10px;">
        <%= Html.ActionLink(Html.Resource("Master, AddStores"), "Stores/" + Model.Client.Id.ToString(), "Chain", (object)Model.Client.Id)%> <br />
<%
        if (Model.HasTemplate)
        {
%>
        <%= Html.ActionLink(Html.Resource("Master, EditWebsite"), "Website/" + Model.Client.Id.ToString(), "Chain", (object)Model.Client.Id)%> <br />
<%
        }
%>
        <%= Html.ActionLink(Html.Resource("Master, Return"), "All", "Chain", (object)Model.Client.Id )%>
        </div>
<%
    }
%>

</asp:Content>
