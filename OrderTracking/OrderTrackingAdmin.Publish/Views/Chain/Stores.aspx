<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.ClientViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Chain Stores
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Add Stores</h2>
    <h3>Tick the stores to add to the chain</h3>
    <div style="margin-top:10px;">
<%
    using (Html.BeginForm("Stores", "Chain", FormMethod.Post))
    {
        foreach (Store store in Model.Stores)
        {%>
            <%= Html.CheckBox("store_" + store.Id.ToString(), false)%>
            <%= store.ExternalStoreId %> / <%= store.Name %>
            <br />
        <%
        }%>
    
        <div style="margin-top:10px;">
            <input type="submit" value="<%=Html.Resource("Master, AddStores")%>" />
        </div>
        <div style="margin-top:10px;">
            <%= Html.ActionLink(Html.Resource("Master, Return"), "Edit/" + Model.Client.Id.ToString(), "Chain", (object)Model.Client.Id)%>
        </div>
    </div>
<%
    }
%>
</asp:Content>
