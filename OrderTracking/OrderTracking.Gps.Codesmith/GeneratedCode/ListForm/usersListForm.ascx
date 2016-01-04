<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.UserListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Username")
					GetMessage("Name")
					GetMessage("Surname")
					GetMessage("Password")
					GetMessage("Email")
					GetMessage("Active")
					GetMessage("Description")
					GetMessage("Sourceaddress")
					GetMessage("Created")
					GetMessage("Botype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((User) Container.DataItem).Username\%>
            </td>
			<td>
                <\%# ((User) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((User) Container.DataItem).Surname\%>
            </td>
			<td>
                <\%# ((User) Container.DataItem).Password\%>
            </td>
			<td>
                <\%# ((User) Container.DataItem).Email\%>
            </td>
			<td>
                <\%# ((User) Container.DataItem).Active\%>
            </td>
			<td>
                <\%# ((User) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((User) Container.DataItem).Sourceaddress\%>
            </td>
			<td>
                <\%# ((User) Container.DataItem).Created\%>
            </td>
			<td>
                <\%# ((User) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




