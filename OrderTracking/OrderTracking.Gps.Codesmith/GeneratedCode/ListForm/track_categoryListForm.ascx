<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackcategoryListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TrackcategoryListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Botype")
					GetMessage("Name")
					GetMessage("Description")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Trackcategory) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Trackcategory) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Trackcategory) Container.DataItem).Description\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




