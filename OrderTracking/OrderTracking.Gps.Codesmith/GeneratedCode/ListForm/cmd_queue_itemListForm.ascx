<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmdqueueitemListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.CmdqueueitemListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Gatecommandid")
					GetMessage("Gatecommandname")
					GetMessage("Stepcurrent")
					GetMessage("Stepmax")
					GetMessage("Stepdesc")
					GetMessage("Errordesc")
					GetMessage("Customstate")
					GetMessage("Deliverystatus")
					GetMessage("Outgoing")
					GetMessage("Timestampclient")
					GetMessage("Timestampqueued")
					GetMessage("Timestampdelivered")
					GetMessage("Timestamplasttry")
					GetMessage("Retrycount")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Gatecommandid\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Gatecommandname\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Stepcurrent\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Stepmax\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Stepdesc\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Errordesc\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Customstate\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Deliverystatus\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Outgoing\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Timestampclient\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Timestampqueued\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Timestampdelivered\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Timestamplasttry\%>
            </td>
			<td>
                <\%# ((Cmdqueueitem) Container.DataItem).Retrycount\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




