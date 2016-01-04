<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LicenseListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.LicenseListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Licensedusers")
					GetMessage("Licenseid")
					GetMessage("Customerid")
					GetMessage("Description")
					GetMessage("Signature")
					GetMessage("Email")
					GetMessage("Botype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((License) Container.DataItem).Licensedusers\%>
            </td>
			<td>
                <\%# ((License) Container.DataItem).Licenseid\%>
            </td>
			<td>
                <\%# ((License) Container.DataItem).Customerid\%>
            </td>
			<td>
                <\%# ((License) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((License) Container.DataItem).Signature\%>
            </td>
			<td>
                <\%# ((License) Container.DataItem).Email\%>
            </td>
			<td>
                <\%# ((License) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




