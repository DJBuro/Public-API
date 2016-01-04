<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ApplicationListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Botype")
					GetMessage("Name")
					GetMessage("Description")
					GetMessage("Maxusers")
					GetMessage("Expire")
					GetMessage("Created")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Application) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Application) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Application) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Application) Container.DataItem).Maxusers\%>
            </td>
			<td>
                <\%# ((Application) Container.DataItem).Expire\%>
            </td>
			<td>
                <\%# ((Application) Container.DataItem).Created\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




