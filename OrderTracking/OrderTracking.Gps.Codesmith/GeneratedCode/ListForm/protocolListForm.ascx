<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProtocolListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ProtocolListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Botype")
					GetMessage("Name")
					GetMessage("Description")
					GetMessage("Adapterbotype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Protocol) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Protocol) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Protocol) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Protocol) Container.DataItem).Adapterbotype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




