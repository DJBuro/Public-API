<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackerListForm.ascx.cs" Inherits="OrderTracking.Data.TrackerListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("BatteryLevel")
					GetMessage("IMEI")
					GetMessage("SerialNumber")
					GetMessage("PhoneNumber")
					GetMessage("Active")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Tracker) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Tracker) Container.DataItem).BatteryLevel\%>
            </td>
			<td>
                <\%# ((Tracker) Container.DataItem).IMEI\%>
            </td>
			<td>
                <\%# ((Tracker) Container.DataItem).SerialNumber\%>
            </td>
			<td>
                <\%# ((Tracker) Container.DataItem).PhoneNumber\%>
            </td>
			<td>
                <\%# ((Tracker) Container.DataItem).Active\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




