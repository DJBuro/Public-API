<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageproviderListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MessageproviderListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Botype")
					GetMessage("Msgprovname")
					GetMessage("Enabled")
					GetMessage("Created")
					GetMessage("Url")
					GetMessage("Username")
					GetMessage("Password")
					GetMessage("Callinterval")
					GetMessage("Customlong")
					GetMessage("Customstring")
					GetMessage("Calltimeout")
					GetMessage("Routelabel")
					GetMessage("Defaultprovider")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Msgprovname\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Enabled\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Created\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Url\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Username\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Password\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Callinterval\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Customlong\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Customstring\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Calltimeout\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Routelabel\%>
            </td>
			<td>
                <\%# ((Messageprovider) Container.DataItem).Defaultprovider\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




