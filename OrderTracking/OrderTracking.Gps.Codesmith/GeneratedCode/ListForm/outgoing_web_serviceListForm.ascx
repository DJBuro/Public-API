<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutgoingwebserviceListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.OutgoingwebserviceListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Namespace")
					GetMessage("Url")
					GetMessage("Username")
					GetMessage("Password")
					GetMessage("Callinterval")
					GetMessage("Customlong")
					GetMessage("Customstring")
					GetMessage("Timeout")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Outgoingwebservice) Container.DataItem).Namespace\%>
            </td>
			<td>
                <\%# ((Outgoingwebservice) Container.DataItem).Url\%>
            </td>
			<td>
                <\%# ((Outgoingwebservice) Container.DataItem).Username\%>
            </td>
			<td>
                <\%# ((Outgoingwebservice) Container.DataItem).Password\%>
            </td>
			<td>
                <\%# ((Outgoingwebservice) Container.DataItem).Callinterval\%>
            </td>
			<td>
                <\%# ((Outgoingwebservice) Container.DataItem).Customlong\%>
            </td>
			<td>
                <\%# ((Outgoingwebservice) Container.DataItem).Customstring\%>
            </td>
			<td>
                <\%# ((Outgoingwebservice) Container.DataItem).Timeout\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




