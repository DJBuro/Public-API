<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.GlobalViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Apn
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.ActionLink("Global Administration", "/", "Global")%> > <%= Model.Apn.Provider %></h2>    
    <table>
<%
    using (Html.BeginForm("SaveApn", "Global", FormMethod.Post))
    {
%>
<input id="Id" name="Id" type="hidden" value="<%=Model.Apn.Id.Value%>" /> 

        <tr class="separator">
            <td colspan="4"></td>
        </tr>
        <tr>
            <td colspan="4"><strong>Provider</strong></td>
        </tr>        
        <tr>
            <td colspan="2"><%=Html.TextBox("Provider", Model.Apn.Provider, new { @size = "30" })%></td><td colspan="2"><%= Model.Apn.ApnTrackers.Count %> Trackers on this APN</td>
        </tr>
        <tr>
            <td><strong>Name</strong></td><td><strong>Username</strong></td><td><strong>Password</strong></td><td></td>
        </tr>        
        <tr>
            <td><%=Html.TextBox("Name", Model.Apn.Name)%></td><td><%=Html.TextBox("Username", Model.Apn.Username)%></td><td><%=Html.TextBox("Password", Model.Apn.Password)%></td><td><input type="submit" value="Update Apn" /></td>
        </tr>
   
<%}//end save
    using (Html.BeginForm("DeleteApn", "Global", FormMethod.Post))
    {
        if (Model.Apn.ApnTrackers.Count == 0)
        {
%>
<input id="Id" name="Id" type="hidden" value="<%=Model.Apn.Id.Value%>" /> 
<input id="Provider" name="Provider" type="hidden" value="<%=Model.Apn.Provider%>" /> 
<input id="Name" name="Name" type="hidden" value="<%=Model.Apn.Name%>" /> 
<input id="Username" name="Username" type="hidden" value="<%=Model.Apn.Username%>" /> 

        <tr>
            <td colspan="3"></td><td><input type="submit" value="Delete Apn" /></td>
        </tr>
<%
        }//end if
    }//end delete %>
</table>
</asp:Content>
