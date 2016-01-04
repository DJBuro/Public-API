<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RamesesAddressListForm.ascx.cs" Inherits="Loyalty.Data.RamesesAddressListForm" %>
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
                <\%# ((RamesesAddress) Container.DataItem).ContactID\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Contact\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).AddressType\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).OptFlag\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).AddressID\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).PostOfficeID\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).SubAddress\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Org1\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Org2\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Org3\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Prem1\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Prem2\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Prem3\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).RoadNum\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).RoadName\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Locality\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Town\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).County\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).PostCode\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Grid\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Refno\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Directions\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Dps\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).PafType\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).Flags\%>
            </td>
			<td>
                <\%# ((RamesesAddress) Container.DataItem).TimesOrdered\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




