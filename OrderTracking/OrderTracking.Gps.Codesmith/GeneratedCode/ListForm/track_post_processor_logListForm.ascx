<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackpostprocessorlogListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TrackpostprocessorlogListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Timestampstarted")
					GetMessage("Timestampstartedms")
					GetMessage("Timestampdone")
					GetMessage("Timestampdonems")
					GetMessage("Trackinfomaxtime")
					GetMessage("Trackinfomaxtimems")
					GetMessage("Dirtycount")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Trackpostprocessorlog) Container.DataItem).Timestampstarted\%>
            </td>
			<td>
                <\%# ((Trackpostprocessorlog) Container.DataItem).Timestampstartedms\%>
            </td>
			<td>
                <\%# ((Trackpostprocessorlog) Container.DataItem).Timestampdone\%>
            </td>
			<td>
                <\%# ((Trackpostprocessorlog) Container.DataItem).Timestampdonems\%>
            </td>
			<td>
                <\%# ((Trackpostprocessorlog) Container.DataItem).Trackinfomaxtime\%>
            </td>
			<td>
                <\%# ((Trackpostprocessorlog) Container.DataItem).Trackinfomaxtimems\%>
            </td>
			<td>
                <\%# ((Trackpostprocessorlog) Container.DataItem).Dirtycount\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




