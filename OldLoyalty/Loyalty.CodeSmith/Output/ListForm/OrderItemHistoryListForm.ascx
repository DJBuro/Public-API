<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderItemHistoryListForm.ascx.cs" Inherits="Loyalty.Data.OrderItemHistoryListForm" %>
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
                <\%# ((OrderItemHistory) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((OrderItemHistory) Container.DataItem).ItemPrice\%>
            </td>
			<td>
                <\%# ((OrderItemHistory) Container.DataItem).ItemLoyaltyPoints\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




