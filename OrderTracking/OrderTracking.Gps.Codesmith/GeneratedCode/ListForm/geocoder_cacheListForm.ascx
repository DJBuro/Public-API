<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeocodercacheListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GeocodercacheListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Lon")
					GetMessage("Lat")
					GetMessage("Countryname")
					GetMessage("Cityname")
					GetMessage("Postalcodenumber")
					GetMessage("Streetname")
					GetMessage("Streetnumber")
					GetMessage("Streetbox")
					GetMessage("Address")
					GetMessage("Lonlathash")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Geocodercache) Container.DataItem).Lon\%>
            </td>
			<td>
                <\%# ((Geocodercache) Container.DataItem).Lat\%>
            </td>
			<td>
                <\%# ((Geocodercache) Container.DataItem).Countryname\%>
            </td>
			<td>
                <\%# ((Geocodercache) Container.DataItem).Cityname\%>
            </td>
			<td>
                <\%# ((Geocodercache) Container.DataItem).Postalcodenumber\%>
            </td>
			<td>
                <\%# ((Geocodercache) Container.DataItem).Streetname\%>
            </td>
			<td>
                <\%# ((Geocodercache) Container.DataItem).Streetnumber\%>
            </td>
			<td>
                <\%# ((Geocodercache) Container.DataItem).Streetbox\%>
            </td>
			<td>
                <\%# ((Geocodercache) Container.DataItem).Address\%>
            </td>
			<td>
                <\%# ((Geocodercache) Container.DataItem).Lonlathash\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




