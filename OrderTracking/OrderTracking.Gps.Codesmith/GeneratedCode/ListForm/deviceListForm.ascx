<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeviceListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.DeviceListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Devicename")
					GetMessage("Botype")
					GetMessage("Created")
					GetMessage("Hideposition")
					GetMessage("Proximity")
					GetMessage("IMEI")
					GetMessage("Phonenumber")
					GetMessage("Lastip")
					GetMessage("Lastport")
					GetMessage("Staticip")
					GetMessage("Staticport")
					GetMessage("Longitude")
					GetMessage("Latitude")
					GetMessage("Groundspeed")
					GetMessage("Altitude")
					GetMessage("Heading")
					GetMessage("Timestamp")
					GetMessage("Milliseconds")
					GetMessage("Protocolid")
					GetMessage("Protocolversionid")
					GetMessage("Valid")
					GetMessage("Apn")
					GetMessage("Gprsusername")
					GetMessage("Gprspassword")
					GetMessage("Devdefid")
					GetMessage("Outgoingtransport")
					GetMessage("Email")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Device) Container.DataItem).Devicename\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Created\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Hideposition\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Proximity\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).IMEI\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Phonenumber\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Lastip\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Lastport\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Staticip\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Staticport\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Longitude\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Latitude\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Groundspeed\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Altitude\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Heading\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Timestamp\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Milliseconds\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Protocolid\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Protocolversionid\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Valid\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Apn\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Gprsusername\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Gprspassword\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Devdefid\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Outgoingtransport\%>
            </td>
			<td>
                <\%# ((Device) Container.DataItem).Email\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




