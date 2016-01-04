<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountListForm.ascx.cs" Inherits="OrderTracking.Data.AccountListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("UserName")
					GetMessage("Password")
					GetMessage("GpsEnabled")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Account) Container.DataItem).UserName\%>
            </td>
			<td>
                <\%# ((Account) Container.DataItem).Password\%>
            </td>
			<td>
                <\%# ((Account) Container.DataItem).GpsEnabled\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




