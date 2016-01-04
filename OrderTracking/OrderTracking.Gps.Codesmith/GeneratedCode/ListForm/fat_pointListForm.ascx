<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FatpointListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.FatpointListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Starttrackdataid")
					GetMessage("Endtrackdataid")
					GetMessage("Longitude")
					GetMessage("Latitude")
					GetMessage("Altitude")
					GetMessage("Starttime")
					GetMessage("Starttimems")
					GetMessage("Endtime")
					GetMessage("Endtimems")
					GetMessage("Errorradius")
					GetMessage("Builddots")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Fatpoint) Container.DataItem).Starttrackdataid\%>
            </td>
			<td>
                <\%# ((Fatpoint) Container.DataItem).Endtrackdataid\%>
            </td>
			<td>
                <\%# ((Fatpoint) Container.DataItem).Longitude\%>
            </td>
			<td>
                <\%# ((Fatpoint) Container.DataItem).Latitude\%>
            </td>
			<td>
                <\%# ((Fatpoint) Container.DataItem).Altitude\%>
            </td>
			<td>
                <\%# ((Fatpoint) Container.DataItem).Starttime\%>
            </td>
			<td>
                <\%# ((Fatpoint) Container.DataItem).Starttimems\%>
            </td>
			<td>
                <\%# ((Fatpoint) Container.DataItem).Endtime\%>
            </td>
			<td>
                <\%# ((Fatpoint) Container.DataItem).Endtimems\%>
            </td>
			<td>
                <\%# ((Fatpoint) Container.DataItem).Errorradius\%>
            </td>
			<td>
                <\%# ((Fatpoint) Container.DataItem).Builddots\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




