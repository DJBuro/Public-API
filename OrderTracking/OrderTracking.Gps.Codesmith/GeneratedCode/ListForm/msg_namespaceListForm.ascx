<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MsgnamespaceListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MsgnamespaceListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Description")
					GetMessage("Protocolid")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Msgnamespace) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Msgnamespace) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Msgnamespace) Container.DataItem).Protocolid\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




