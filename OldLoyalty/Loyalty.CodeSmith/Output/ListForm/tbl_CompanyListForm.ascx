<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblCompanyListForm.ascx.cs" Inherits="Loyalty.Data.TblCompanyListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("RedemptionRatio")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((TblCompany) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((TblCompany) Container.DataItem).RedemptionRatio\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




