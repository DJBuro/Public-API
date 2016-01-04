<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttributeListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.AttributeListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Attributename")
					GetMessage("Attributetype")
					GetMessage("Intvalue")
					GetMessage("Doublevalue")
					GetMessage("Stringvalue")
					GetMessage("Boolvalue")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Attribute) Container.DataItem).Attributename\%>
            </td>
			<td>
                <\%# ((Attribute) Container.DataItem).Attributetype\%>
            </td>
			<td>
                <\%# ((Attribute) Container.DataItem).Intvalue\%>
            </td>
			<td>
                <\%# ((Attribute) Container.DataItem).Doublevalue\%>
            </td>
			<td>
                <\%# ((Attribute) Container.DataItem).Stringvalue\%>
            </td>
			<td>
                <\%# ((Attribute) Container.DataItem).Boolvalue\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




