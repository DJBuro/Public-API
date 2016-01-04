<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoyaltyCardListForm.ascx.cs" Inherits="Loyalty.Data.LoyaltyCardListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("CardNumber")
					GetMessage("DateTimeCreated")
					GetMessage("Pin")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((LoyaltyCard) Container.DataItem).CardNumber\%>
            </td>
			<td>
                <\%# ((LoyaltyCard) Container.DataItem).DateTimeCreated\%>
            </td>
			<td>
                <\%# ((LoyaltyCard) Container.DataItem).Pin\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




