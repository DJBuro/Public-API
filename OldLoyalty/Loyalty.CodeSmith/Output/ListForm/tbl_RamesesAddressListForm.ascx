<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblRamesesAddressListForm.ascx.cs" Inherits="Loyalty.Data.TblRamesesAddressListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("ContactID")
					GetMessage("Contact")
					GetMessage("AddressType")
					GetMessage("OptFlag")
					GetMessage("AddressID")
					GetMessage("PostOfficeID")
					GetMessage("SubAddress")
					GetMessage("Org1")
					GetMessage("Org2")
					GetMessage("Org3")
					GetMessage("Prem1")
					GetMessage("Prem2")
					GetMessage("Prem3")
					GetMessage("RoadNum")
					GetMessage("RoadName")
					GetMessage("Locality")
					GetMessage("Town")
					GetMessage("County")
					GetMessage("PostCode")
					GetMessage("Grid")
					GetMessage("Refno")
					GetMessage("Directions")
					GetMessage("Dps")
					GetMessage("PafType")
					GetMessage("Flags")
					GetMessage("TimesOrdered")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).ContactID\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Contact\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).AddressType\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).OptFlag\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).AddressID\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).PostOfficeID\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).SubAddress\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Org1\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Org2\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Org3\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Prem1\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Prem2\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Prem3\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).RoadNum\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).RoadName\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Locality\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Town\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).County\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).PostCode\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Grid\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Refno\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Directions\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Dps\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).PafType\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).Flags\%>
            </td>
			<td>
                <\%# ((TblRamesesAddress) Container.DataItem).TimesOrdered\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




