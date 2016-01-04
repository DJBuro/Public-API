<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountAddressListForm.ascx.cs" Inherits="Loyalty.Data.AccountAddressListForm" %>
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
                <\%# ((AccountAddress) Container.DataItem).AddressLineOne\%>
            </td>
			<td>
                <\%# ((AccountAddress) Container.DataItem).AddressLineTwo\%>
            </td>
			<td>
                <\%# ((AccountAddress) Container.DataItem).AddressLineThree\%>
            </td>
			<td>
                <\%# ((AccountAddress) Container.DataItem).AddressLineFour\%>
            </td>
			<td>
                <\%# ((AccountAddress) Container.DataItem).TownCity\%>
            </td>
			<td>
                <\%# ((AccountAddress) Container.DataItem).CountyProvince\%>
            </td>
			<td>
                <\%# ((AccountAddress) Container.DataItem).PostCode\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




