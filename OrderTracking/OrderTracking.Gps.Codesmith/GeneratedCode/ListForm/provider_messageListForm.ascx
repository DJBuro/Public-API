<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProvidermessageListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ProvidermessageListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Clientdeviceid")
					GetMessage("Clientaddress")
					GetMessage("Message")
					GetMessage("Deliverystatus")
					GetMessage("Outgoing")
					GetMessage("Timestampclient")
					GetMessage("Timestampqueued")
					GetMessage("Timestampdelivered")
					GetMessage("Timestamplasttry")
					GetMessage("Retrycount")
					GetMessage("Transport")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Providermessage) Container.DataItem).Clientdeviceid\%>
            </td>
			<td>
                <\%# ((Providermessage) Container.DataItem).Clientaddress\%>
            </td>
			<td>
                <\%# ((Providermessage) Container.DataItem).Message\%>
            </td>
			<td>
                <\%# ((Providermessage) Container.DataItem).Deliverystatus\%>
            </td>
			<td>
                <\%# ((Providermessage) Container.DataItem).Outgoing\%>
            </td>
			<td>
                <\%# ((Providermessage) Container.DataItem).Timestampclient\%>
            </td>
			<td>
                <\%# ((Providermessage) Container.DataItem).Timestampqueued\%>
            </td>
			<td>
                <\%# ((Providermessage) Container.DataItem).Timestampdelivered\%>
            </td>
			<td>
                <\%# ((Providermessage) Container.DataItem).Timestamplasttry\%>
            </td>
			<td>
                <\%# ((Providermessage) Container.DataItem).Retrycount\%>
            </td>
			<td>
                <\%# ((Providermessage) Container.DataItem).Transport\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




