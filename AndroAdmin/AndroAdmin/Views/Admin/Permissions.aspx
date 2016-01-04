<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroAdminViewData.AdminViewData>" %>
<%@ Import Namespace="AndroAdmin.Mvc.Extensions"%>
<%@ Import Namespace="AndroAdmin.Dao.Domain"%>
<%@ Import Namespace="AndroAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User Permissions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "Index", "Home")%> > <%= Html.ActionLink("Users", "Index", "Admin")%> > <%= Html.ActionLink(Model.AndroEditUser.FirstName +" "+ Model.AndroEditUser.SurName, "EditUser/" + Model.AndroEditUser.Id.Value, "Admin")%> > Permissions</h2>




<input id="Id" name="Id" type="hidden" value="<%=Model.AndroEditUser.Id%>" />


<table>        
    <tr class="separator">
        <td colspan="4"></td>
    </tr>
    <%
        
        var userPermissions = Model.AndroEditUser.UserPermissions;


        for (int i = 0; i < Model.Projects.Count; i++)
        {
            var active = false;
            
            foreach (AndroUserPermission list in userPermissions)
            {
                if(list.Project.Id == Model.Projects[i].Id)
                    active = true;
            }
        
        %>
            
        <tr>
            <td  align="left" class="<%= active %>"></td><td><%= Model.Projects[i].Name%></td><td><%= active ? (Html.ActionLink("Remove", "RemovePermissions/" + Model.AndroEditUser.Id.Value + "/" + Model.Projects[i].Id.Value, "Admin")) : (Html.ActionLink("Add", "AddPermissions/" + Model.AndroEditUser.Id.Value + "/" + Model.Projects[i].Id.Value, "Admin"))%></td><td></td>
        </tr>
            
        <%}%>   

            <tr class="separator">
        <td colspan="4"></td>
    </tr>
</table>

</asp:Content>
