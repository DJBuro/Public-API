<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobilenetworkListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MobilenetworkListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Operator")
					GetMessage("Username")
					GetMessage("Password")
					GetMessage("Apn")
					GetMessage("Dns1")
					GetMessage("Dns2")
					GetMessage("Description")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Mobilenetwork) Container.DataItem).Operator\%>
            </td>
			<td>
                <\%# ((Mobilenetwork) Container.DataItem).Username\%>
            </td>
			<td>
                <\%# ((Mobilenetwork) Container.DataItem).Password\%>
            </td>
			<td>
                <\%# ((Mobilenetwork) Container.DataItem).Apn\%>
            </td>
			<td>
                <\%# ((Mobilenetwork) Container.DataItem).Dns1\%>
            </td>
			<td>
                <\%# ((Mobilenetwork) Container.DataItem).Dns2\%>
            </td>
			<td>
                <\%# ((Mobilenetwork) Container.DataItem).Description\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




