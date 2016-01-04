<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoyaltyAccountListForm.ascx.cs" Inherits="Loyalty.Data.LoyaltyAccountListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Points")
					GetMessage("DateTimeCreated")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((LoyaltyAccount) Container.DataItem).Points\%>
            </td>
			<td>
                <\%# ((LoyaltyAccount) Container.DataItem).DateTimeCreated\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




