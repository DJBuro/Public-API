<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblLoyaltyAccountStatusListForm.ascx.cs" Inherits="Loyalty.Data.TblLoyaltyAccountStatusListForm" %>
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
                <\%# ((TblLoyaltyAccountStatus) Container.DataItem).Reason\%>
            </td>
			<td>
                <\%# ((TblLoyaltyAccountStatus) Container.DataItem).DateTimeCreated\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




