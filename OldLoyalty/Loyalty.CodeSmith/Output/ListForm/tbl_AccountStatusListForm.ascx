<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblAccountStatusListForm.ascx.cs" Inherits="Loyalty.Data.TblAccountStatusListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Description")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((TblAccountStatus) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((TblAccountStatus) Container.DataItem).Description\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




