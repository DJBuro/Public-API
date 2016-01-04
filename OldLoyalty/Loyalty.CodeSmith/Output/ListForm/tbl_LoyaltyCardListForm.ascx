<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblLoyaltyCardListForm.ascx.cs" Inherits="Loyalty.Data.TblLoyaltyCardListForm" %>
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
                <\%# ((TblLoyaltyCard) Container.DataItem).CardNumber\%>
            </td>
			<td>
                <\%# ((TblLoyaltyCard) Container.DataItem).DateTimeCreated\%>
            </td>
			<td>
                <\%# ((TblLoyaltyCard) Container.DataItem).Pin\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




