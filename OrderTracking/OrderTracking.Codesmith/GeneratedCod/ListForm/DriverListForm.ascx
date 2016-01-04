<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DriverListForm.ascx.cs" Inherits="OrderTracking.Data.DriverListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("ExternalDriverId")
					GetMessage("Vehicle")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Driver) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Driver) Container.DataItem).ExternalDriverId\%>
            </td>
			<td>
                <\%# ((Driver) Container.DataItem).Vehicle\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




