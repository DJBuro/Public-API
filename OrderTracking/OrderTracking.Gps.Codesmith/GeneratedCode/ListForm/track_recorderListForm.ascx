<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackrecorderListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TrackrecorderListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Botype")
					GetMessage("Name")
					GetMessage("Trackinfoid")
					GetMessage("Recording")
					GetMessage("Lasttrackdataid")
					GetMessage("Timefilter")
					GetMessage("Distancefilter")
					GetMessage("Directionfilter")
					GetMessage("Directionthreshold")
					GetMessage("Speedfilter")
					GetMessage("Restarttime")
					GetMessage("Restartdistance")
					GetMessage("Trackcategoryid")
					GetMessage("Lastuncertaintrackdataid")
					GetMessage("Restartinterval")
					GetMessage("Restartintervaloffset")
					GetMessage("Smstimefilter")
					GetMessage("Motion")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Trackinfoid\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Recording\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Lasttrackdataid\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Timefilter\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Distancefilter\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Directionfilter\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Directionthreshold\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Speedfilter\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Restarttime\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Restartdistance\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Trackcategoryid\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Lastuncertaintrackdataid\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Restartinterval\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Restartintervaloffset\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Smstimefilter\%>
            </td>
			<td>
                <\%# ((Trackrecorder) Container.DataItem).Motion\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




