<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProtocolversionListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ProtocolversionListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Botype")
					GetMessage("Protocolid")
					GetMessage("Majorversion")
					GetMessage("Minorversion")
					GetMessage("Clientname")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Protocolversion) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Protocolversion) Container.DataItem).Protocolid\%>
            </td>
			<td>
                <\%# ((Protocolversion) Container.DataItem).Majorversion\%>
            </td>
			<td>
                <\%# ((Protocolversion) Container.DataItem).Minorversion\%>
            </td>
			<td>
                <\%# ((Protocolversion) Container.DataItem).Clientname\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




