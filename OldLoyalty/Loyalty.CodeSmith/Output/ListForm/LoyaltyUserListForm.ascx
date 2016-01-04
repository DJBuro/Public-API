<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoyaltyUserListForm.ascx.cs" Inherits="Loyalty.Data.LoyaltyUserListForm" %>
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
                <\%# ((LoyaltyUser) Container.DataItem).FirstName\%>
            </td>
			<td>
                <\%# ((LoyaltyUser) Container.DataItem).MiddleInitial\%>
            </td>
			<td>
                <\%# ((LoyaltyUser) Container.DataItem).SurName\%>
            </td>
			<td>
                <\%# ((LoyaltyUser) Container.DataItem).DateTimeCreated\%>
            </td>
			<td>
                <\%# ((LoyaltyUser) Container.DataItem).EmailAddress\%>
            </td>
			<td>
                <\%# ((LoyaltyUser) Container.DataItem).Password\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




