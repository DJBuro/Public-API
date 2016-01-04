<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListenerListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ListenerListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Enabled")
					GetMessage("Serveraddress")
					GetMessage("Serverport")
					GetMessage("Botype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Listener) Container.DataItem).Enabled\%>
            </td>
			<td>
                <\%# ((Listener) Container.DataItem).Serveraddress\%>
            </td>
			<td>
                <\%# ((Listener) Container.DataItem).Serverport\%>
            </td>
			<td>
                <\%# ((Listener) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




