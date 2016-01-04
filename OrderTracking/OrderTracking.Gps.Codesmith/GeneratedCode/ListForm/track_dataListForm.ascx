<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackdataListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TrackdataListForm" %>
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
					GetMessage("Distancenext")
					GetMessage("Valid")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Trackdata) Container.DataItem).Longitude\%>
            </td>
			<td>
                <\%# ((Trackdata) Container.DataItem).Latitude\%>
            </td>
			<td>
                <\%# ((Trackdata) Container.DataItem).Altitude\%>
            </td>
			<td>
                <\%# ((Trackdata) Container.DataItem).Heading\%>
            </td>
			<td>
                <\%# ((Trackdata) Container.DataItem).Groundspeed\%>
            </td>
			<td>
                <\%# ((Trackdata) Container.DataItem).Timestamp\%>
            </td>
			<td>
                <\%# ((Trackdata) Container.DataItem).Milliseconds\%>
            </td>
			<td>
                <\%# ((Trackdata) Container.DataItem).Distancenext\%>
            </td>
			<td>
                <\%# ((Trackdata) Container.DataItem).Valid\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




