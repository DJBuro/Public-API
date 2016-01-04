<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblLoyaltyUserListForm.ascx.cs" Inherits="Loyalty.Data.TblLoyaltyUserListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("FirstName")
					GetMessage("MiddleInitial")
					GetMessage("SurName")
					GetMessage("DateTimeCreated")
					GetMessage("EmailAddress")
					GetMessage("Password")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((TblLoyaltyUser) Container.DataItem).FirstName\%>
            </td>
			<td>
                <\%# ((TblLoyaltyUser) Container.DataItem).MiddleInitial\%>
            </td>
			<td>
                <\%# ((TblLoyaltyUser) Container.DataItem).SurName\%>
            </td>
			<td>
                <\%# ((TblLoyaltyUser) Container.DataItem).DateTimeCreated\%>
            </td>
			<td>
                <\%# ((TblLoyaltyUser) Container.DataItem).EmailAddress\%>
            </td>
			<td>
                <\%# ((TblLoyaltyUser) Container.DataItem).Password\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




