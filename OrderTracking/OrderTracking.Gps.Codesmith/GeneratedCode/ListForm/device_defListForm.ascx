<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DevicedefListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.DevicedefListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Devicename")
					GetMessage("Description")
					GetMessage("Templatemsgfielddictid")
					GetMessage("Botype")
					GetMessage("Upgradable")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Devicedef) Container.DataItem).Devicename\%>
            </td>
			<td>
                <\%# ((Devicedef) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Devicedef) Container.DataItem).Templatemsgfielddictid\%>
            </td>
			<td>
                <\%# ((Devicedef) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Devicedef) Container.DataItem).Upgradable\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




