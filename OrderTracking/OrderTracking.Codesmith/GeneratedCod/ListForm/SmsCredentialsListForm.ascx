<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmsCredentialListForm.ascx.cs" Inherits="OrderTracking.Data.SmsCredentialListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Username")
					GetMessage("Password")
					GetMessage("SmsFrom")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((SmsCredential) Container.DataItem).Username\%>
            </td>
			<td>
                <\%# ((SmsCredential) Container.DataItem).Password\%>
            </td>
			<td>
                <\%# ((SmsCredential) Container.DataItem).SmsFrom\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




