<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblTransactionHistoryListForm.ascx.cs" Inherits="Loyalty.Data.TblTransactionHistoryListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("DateTimeOrdered")
					GetMessage("OrderId")
					GetMessage("OrderTypeId")
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
                <\%# ((TblTransactionHistory) Container.DataItem).DateTimeOrdered\%>
            </td>
			<td>
                <\%# ((TblTransactionHistory) Container.DataItem).OrderId\%>
            </td>
			<td>
                <\%# ((TblTransactionHistory) Container.DataItem).OrderTypeId\%>
            </td>
			<td>
                <\%# ((TblTransactionHistory) Container.DataItem).LoyaltyPointsRedeemed\%>
            </td>
			<td>
                <\%# ((TblTransactionHistory) Container.DataItem).LoyaltyPointsAdded\%>
            </td>
			<td>
                <\%# ((TblTransactionHistory) Container.DataItem).LoyaltyPointsValue\%>
            </td>
			<td>
                <\%# ((TblTransactionHistory) Container.DataItem).OrderTotal\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




