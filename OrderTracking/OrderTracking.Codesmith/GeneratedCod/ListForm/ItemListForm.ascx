<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemListForm.ascx.cs" Inherits="OrderTracking.Data.ItemListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Quantity")
					GetMessage("Name")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Item) Container.DataItem).Quantity\%>
            </td>
			<td>
                <\%# ((Item) Container.DataItem).Name\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




