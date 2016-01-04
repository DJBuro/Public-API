<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StoreListForm.ascx.cs" Inherits="OrderTracking.Data.StoreListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("ExternalStoreId")
					GetMessage("Name")
					GetMessage("DeliveryRadius")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Store) Container.DataItem).ExternalStoreId\%>
            </td>
			<td>
                <\%# ((Store) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Store) Container.DataItem).DeliveryRadius\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




