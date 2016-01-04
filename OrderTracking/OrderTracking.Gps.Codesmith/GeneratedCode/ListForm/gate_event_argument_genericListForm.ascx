<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventargumentgenericListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventargumentgenericListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Intvalue")
					GetMessage("Dblvalue")
					GetMessage("Boolvalue")
					GetMessage("Strvalue")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gateeventargumentgeneric) Container.DataItem).Intvalue\%>
            </td>
			<td>
                <\%# ((Gateeventargumentgeneric) Container.DataItem).Dblvalue\%>
            </td>
			<td>
                <\%# ((Gateeventargumentgeneric) Container.DataItem).Boolvalue\%>
            </td>
			<td>
                <\%# ((Gateeventargumentgeneric) Container.DataItem).Strvalue\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




