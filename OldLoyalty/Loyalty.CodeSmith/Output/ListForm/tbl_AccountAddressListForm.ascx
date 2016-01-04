<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblAccountAddressListForm.ascx.cs" Inherits="Loyalty.Data.TblAccountAddressListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("AddressLineOne")
					GetMessage("AddressLineTwo")
					GetMessage("AddressLineThree")
					GetMessage("AddressLineFour")
					GetMessage("TownCity")
					GetMessage("CountyProvince")
					GetMessage("PostCode")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((TblAccountAddress) Container.DataItem).AddressLineOne\%>
            </td>
			<td>
                <\%# ((TblAccountAddress) Container.DataItem).AddressLineTwo\%>
            </td>
			<td>
                <\%# ((TblAccountAddress) Container.DataItem).AddressLineThree\%>
            </td>
			<td>
                <\%# ((TblAccountAddress) Container.DataItem).AddressLineFour\%>
            </td>
			<td>
                <\%# ((TblAccountAddress) Container.DataItem).TownCity\%>
            </td>
			<td>
                <\%# ((TblAccountAddress) Container.DataItem).CountyProvince\%>
            </td>
			<td>
                <\%# ((TblAccountAddress) Container.DataItem).PostCode\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




