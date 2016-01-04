<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoyaltyAccountStatusListForm.ascx.cs" Inherits="Loyalty.Data.LoyaltyAccountStatusListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Reason")
					GetMessage("DateTimeCreated")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((LoyaltyAccountStatus) Container.DataItem).Reason\%>
            </td>
			<td>
                <\%# ((LoyaltyAccountStatus) Container.DataItem).DateTimeCreated\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




