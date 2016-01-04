<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotifierListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.NotifierListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Botype")
					GetMessage("Created")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Notifier) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Notifier) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Notifier) Container.DataItem).Created\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




