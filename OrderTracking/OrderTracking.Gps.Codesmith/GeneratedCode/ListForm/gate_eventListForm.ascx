<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Userid")
					GetMessage("Cause")
					GetMessage("Gateeventexpressionevaluatorid")
					GetMessage("Stage")
					GetMessage("Minlongitude")
					GetMessage("Maxlongitude")
					GetMessage("Minlatitude")
					GetMessage("Maxlatitude")
					GetMessage("Minaltitude")
					GetMessage("Maxaltitude")
					GetMessage("Mintimestamp")
					GetMessage("Minmilliseconds")
					GetMessage("Maxtimestamp")
					GetMessage("Maxmilliseconds")
					GetMessage("Scheduled")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gateevent) Container.DataItem).Userid\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Cause\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Gateeventexpressionevaluatorid\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Stage\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Minlongitude\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Maxlongitude\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Minlatitude\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Maxlatitude\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Minaltitude\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Maxaltitude\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Mintimestamp\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Minmilliseconds\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Maxtimestamp\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Maxmilliseconds\%>
            </td>
			<td>
                <\%# ((Gateevent) Container.DataItem).Scheduled\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




