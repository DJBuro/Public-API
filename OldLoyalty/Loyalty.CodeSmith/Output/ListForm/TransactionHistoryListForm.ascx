<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionHistoryListForm.ascx.cs" Inherits="Loyalty.Data.TransactionHistoryListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("DateTimeOrdered")
					GetMessage("OrderId")
					GetMessage("LoyaltyPointsRedeemed")
					GetMessage("LoyaltyPointsAdded")
					GetMessage("LoyaltyPointsValue")
					GetMessage("OrderTotal")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((TransactionHistory) Container.DataItem).DateTimeOrdered\%>
            </td>
			<td>
                <\%# ((TransactionHistory) Container.DataItem).OrderId\%>
            </td>
			<td>
                <\%# ((TransactionHistory) Container.DataItem).LoyaltyPointsRedeemed\%>
            </td>
			<td>
                <\%# ((TransactionHistory) Container.DataItem).LoyaltyPointsAdded\%>
            </td>
			<td>
                <\%# ((TransactionHistory) Container.DataItem).LoyaltyPointsValue\%>
            </td>
			<td>
                <\%# ((TransactionHistory) Container.DataItem).OrderTotal\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




