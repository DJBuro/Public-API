<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderStatusListForm.ascx.cs" Inherits="OrderTracking.Data.OrderStatusListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Processor")
					GetMessage("Time")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((OrderStatus) Container.DataItem).Processor\%>
            </td>
			<td>
                <\%# ((OrderStatus) Container.DataItem).Time\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




