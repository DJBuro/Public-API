<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackdatamodListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TrackdatamodListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Longitude")
					GetMessage("Latitude")
					GetMessage("Altitude")
					GetMessage("Heading")
					GetMessage("Groundspeed")
					GetMessage("Timestamp")
					GetMessage("Milliseconds")
					GetMessage("Valid")
					GetMessage("Deleted")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Trackdatamod) Container.DataItem).Longitude\%>
            </td>
			<td>
                <\%# ((Trackdatamod) Container.DataItem).Latitude\%>
            </td>
			<td>
                <\%# ((Trackdatamod) Container.DataItem).Altitude\%>
            </td>
			<td>
                <\%# ((Trackdatamod) Container.DataItem).Heading\%>
            </td>
			<td>
                <\%# ((Trackdatamod) Container.DataItem).Groundspeed\%>
            </td>
			<td>
                <\%# ((Trackdatamod) Container.DataItem).Timestamp\%>
            </td>
			<td>
                <\%# ((Trackdatamod) Container.DataItem).Milliseconds\%>
            </td>
			<td>
                <\%# ((Trackdatamod) Container.DataItem).Valid\%>
            </td>
			<td>
                <\%# ((Trackdatamod) Container.DataItem).Deleted\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




