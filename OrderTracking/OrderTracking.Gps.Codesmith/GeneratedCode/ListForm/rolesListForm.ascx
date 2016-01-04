<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.RoleListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Rolename")
					GetMessage("Roledescription")
					GetMessage("Botype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Role) Container.DataItem).Rolename\%>
            </td>
			<td>
                <\%# ((Role) Container.DataItem).Roledescription\%>
            </td>
			<td>
                <\%# ((Role) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




