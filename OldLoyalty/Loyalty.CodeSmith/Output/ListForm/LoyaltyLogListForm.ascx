<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoyaltyLogListForm.ascx.cs" Inherits="Loyalty.Data.LoyaltyLogListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("DateTimeCreated")
					GetMessage("SiteId")
					GetMessage("Severity")
					GetMessage("Message")
					GetMessage("Method")
					GetMessage("Variables")
					GetMessage("Source")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((LoyaltyLog) Container.DataItem).DateTimeCreated\%>
            </td>
			<td>
                <\%# ((LoyaltyLog) Container.DataItem).SiteId\%>
            </td>
			<td>
                <\%# ((LoyaltyLog) Container.DataItem).Severity\%>
            </td>
			<td>
                <\%# ((LoyaltyLog) Container.DataItem).Message\%>
            </td>
			<td>
                <\%# ((LoyaltyLog) Container.DataItem).Method\%>
            </td>
			<td>
                <\%# ((LoyaltyLog) Container.DataItem).Variables\%>
            </td>
			<td>
                <\%# ((LoyaltyLog) Container.DataItem).Source\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




