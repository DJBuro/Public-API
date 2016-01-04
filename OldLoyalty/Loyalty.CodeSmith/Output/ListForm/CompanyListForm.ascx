<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyListForm.ascx.cs" Inherits="Loyalty.Data.CompanyListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("RamesesHeadOfficeId")
					GetMessage("RamesesCompanyId")
					GetMessage("Name")
					GetMessage("RedemptionRatio")
					GetMessage("CompanyKey")
					GetMessage("CompanyPassword")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Company) Container.DataItem).RamesesHeadOfficeId\%>
            </td>
			<td>
                <\%# ((Company) Container.DataItem).RamesesCompanyId\%>
            </td>
			<td>
                <\%# ((Company) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Company) Container.DataItem).RedemptionRatio\%>
            </td>
			<td>
                <\%# ((Company) Container.DataItem).CompanyKey\%>
            </td>
			<td>
                <\%# ((Company) Container.DataItem).CompanyPassword\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




