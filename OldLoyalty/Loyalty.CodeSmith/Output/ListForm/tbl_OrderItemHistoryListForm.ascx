<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblOrderItemHistoryListForm.ascx.cs" Inherits="Loyalty.Data.TblOrderItemHistoryListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("ItemPrice")
					GetMessage("ItemLoyaltyPoints")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((TblOrderItemHistory) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((TblOrderItemHistory) Container.DataItem).ItemPrice\%>
            </td>
			<td>
                <\%# ((TblOrderItemHistory) Container.DataItem).ItemLoyaltyPoints\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




