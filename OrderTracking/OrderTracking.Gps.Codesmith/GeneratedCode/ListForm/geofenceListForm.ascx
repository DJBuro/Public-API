<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeofenceListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GeofenceListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Geofencename")
					GetMessage("Geofencedescription")
					GetMessage("Minlongitude")
					GetMessage("Maxlongitude")
					GetMessage("Minlatitude")
					GetMessage("Maxlatitude")
					GetMessage("Minaltitude")
					GetMessage("Maxaltitude")
					GetMessage("Botype")
					GetMessage("Created")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Geofence) Container.DataItem).Geofencename\%>
            </td>
			<td>
                <\%# ((Geofence) Container.DataItem).Geofencedescription\%>
            </td>
			<td>
                <\%# ((Geofence) Container.DataItem).Minlongitude\%>
            </td>
			<td>
                <\%# ((Geofence) Container.DataItem).Maxlongitude\%>
            </td>
			<td>
                <\%# ((Geofence) Container.DataItem).Minlatitude\%>
            </td>
			<td>
                <\%# ((Geofence) Container.DataItem).Maxlatitude\%>
            </td>
			<td>
                <\%# ((Geofence) Container.DataItem).Minaltitude\%>
            </td>
			<td>
                <\%# ((Geofence) Container.DataItem).Maxaltitude\%>
            </td>
			<td>
                <\%# ((Geofence) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Geofence) Container.DataItem).Created\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




