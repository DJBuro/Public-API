<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.TrackerViewData>" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Setup
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h2><%= Html.ActionLink(Html.Resource("Master, Trackers"), "/", "Trackers")%> > <%= Html.ActionLink("Tracker " + Model.Tracker.Name, "/Tracker/" + Model.Tracker.Name, "Trackers")%>  > Setup</h2>

        <table>        
        <%
            using (Html.BeginForm("RunSetupCommands", "Trackers", FormMethod.Post))
            {
        %>
        <input id="id" name="id" type="hidden" value="<%=Model.Tracker.Name %>" />
            <tr class="separator">
                <td colspan="3"></td>
            </tr>
             <tr>
                <td colspan="3">Reply Phone #</td>
            </tr>
            <tr>
                <td colspan="2"><%= Html.TextBox("Phone")%></td><td><input type="submit" value="Send All Commands" /></td>
            </tr>
            <%} // end form %>      
            <tr class="separator">
                <td colspan="3"></td>
            </tr>
            <tr>
                <td><strong><%= Model.TrackerCommands.Count %> Setup Commands</strong></td><td colspan="2"><strong>Reply Phone #</strong></td>
            </tr>
            <%
                foreach (var command in Model.TrackerCommands)
                {
                    var cmd = new StringBuilder();

                    cmd.Append(command.Command);
                    
                    if(command.Command.Contains("{deviceId}"))
                        cmd.Replace("{deviceId}", Model.Tracker.Name);

                    if (command.Command.Contains("{apnName}"))
                        cmd.Replace("{apnName}", Model.Tracker.Apn.Name);

                    if (command.Command.Contains("{apnUserName}"))
                        cmd.Replace("{apnUserName}", Model.Tracker.Apn.Username);

                    if (command.Command.Contains("{apnPassword}"))
                        cmd.Replace("{apnPassword}", Model.Tracker.Apn.Password);

                    using (Html.BeginForm("RunSingleSetupCommands", "Trackers", FormMethod.Post))
                    {
%>
        <input id="id" name="id" type="hidden" value="<%=Model.Tracker.Name %>" />
        <%=Html.Hidden("cmdId", command.Id)%>
            <tr>
                <td><%=command.Name%></td><td><%= Html.TextBox("Phone")%></td><td><input type="submit" value="Send Command" /></td>
            </tr>
                    <%
                    }//form
                }// command.
                 %>
        </table>
        
</asp:Content>
