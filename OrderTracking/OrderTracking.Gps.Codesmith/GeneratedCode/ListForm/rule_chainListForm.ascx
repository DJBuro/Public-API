<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RulechainListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.RulechainListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Description")
					GetMessage("Botype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Rulechain) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Rulechain) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Rulechain) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




