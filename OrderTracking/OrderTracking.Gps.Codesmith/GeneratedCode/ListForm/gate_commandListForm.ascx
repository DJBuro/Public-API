<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GatecommandListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GatecommandListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Description")
					GetMessage("Enabled")
					GetMessage("Namespace")
					GetMessage("Outgoing")
					GetMessage("Special")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gatecommand) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Gatecommand) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Gatecommand) Container.DataItem).Enabled\%>
            </td>
			<td>
                <\%# ((Gatecommand) Container.DataItem).Namespace\%>
            </td>
			<td>
                <\%# ((Gatecommand) Container.DataItem).Outgoing\%>
            </td>
			<td>
                <\%# ((Gatecommand) Container.DataItem).Special\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




