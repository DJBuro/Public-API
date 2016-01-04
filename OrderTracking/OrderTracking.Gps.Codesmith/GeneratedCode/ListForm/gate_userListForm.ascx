<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateuserListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateuserListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Longitude")
					GetMessage("Latitude")
					GetMessage("Groundspeed")
					GetMessage("Altitude")
					GetMessage("Heading")
					GetMessage("Timestamp")
					GetMessage("Servertimestamp")
					GetMessage("Deviceactivity")
					GetMessage("Delay")
					GetMessage("Lasttransport")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gateuser) Container.DataItem).Longitude\%>
            </td>
			<td>
                <\%# ((Gateuser) Container.DataItem).Latitude\%>
            </td>
			<td>
                <\%# ((Gateuser) Container.DataItem).Groundspeed\%>
            </td>
			<td>
                <\%# ((Gateuser) Container.DataItem).Altitude\%>
            </td>
			<td>
                <\%# ((Gateuser) Container.DataItem).Heading\%>
            </td>
			<td>
                <\%# ((Gateuser) Container.DataItem).Timestamp\%>
            </td>
			<td>
                <\%# ((Gateuser) Container.DataItem).Servertimestamp\%>
            </td>
			<td>
                <\%# ((Gateuser) Container.DataItem).Deviceactivity\%>
            </td>
			<td>
                <\%# ((Gateuser) Container.DataItem).Delay\%>
            </td>
			<td>
                <\%# ((Gateuser) Container.DataItem).Lasttransport\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




