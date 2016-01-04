<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerListForm.ascx.cs" Inherits="OrderTracking.Data.CustomerListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("ExternalId")
					GetMessage("Name")
					GetMessage("Credentials")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Customer) Container.DataItem).ExternalId\%>
            </td>
			<td>
                <\%# ((Customer) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Customer) Container.DataItem).Credentials\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




