<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MsgfieldListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MsgfieldListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Type")
					GetMessage("Name")
					GetMessage("Description")
					GetMessage("Localizationkey")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Msgfield) Container.DataItem).Type\%>
            </td>
			<td>
                <\%# ((Msgfield) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Msgfield) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Msgfield) Container.DataItem).Localizationkey\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




