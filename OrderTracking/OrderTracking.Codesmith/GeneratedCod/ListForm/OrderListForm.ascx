<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderListForm.ascx.cs" Inherits="OrderTracking.Data.OrderListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("ExternalOrderId")
					GetMessage("Name")
					GetMessage("TicketNumber")
					GetMessage("ExtraInformation")
					GetMessage("ProximityDelivered")
					GetMessage("Created")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Order) Container.DataItem).ExternalOrderId\%>
            </td>
			<td>
                <\%# ((Order) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Order) Container.DataItem).TicketNumber\%>
            </td>
			<td>
                <\%# ((Order) Container.DataItem).ExtraInformation\%>
            </td>
			<td>
                <\%# ((Order) Container.DataItem).ProximityDelivered\%>
            </td>
			<td>
                <\%# ((Order) Container.DataItem).Created\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




