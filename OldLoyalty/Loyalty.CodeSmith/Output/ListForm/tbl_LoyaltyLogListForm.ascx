<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblLoyaltyLogListForm.ascx.cs" Inherits="Loyalty.Data.TblLoyaltyLogListForm" %>
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
                <\%# ((TblLoyaltyLog) Container.DataItem).DateTimeCreated\%>
            </td>
			<td>
                <\%# ((TblLoyaltyLog) Container.DataItem).SiteId\%>
            </td>
			<td>
                <\%# ((TblLoyaltyLog) Container.DataItem).Severity\%>
            </td>
			<td>
                <\%# ((TblLoyaltyLog) Container.DataItem).Message\%>
            </td>
			<td>
                <\%# ((TblLoyaltyLog) Container.DataItem).Method\%>
            </td>
			<td>
                <\%# ((TblLoyaltyLog) Container.DataItem).Variables\%>
            </td>
			<td>
                <\%# ((TblLoyaltyLog) Container.DataItem).Source\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




