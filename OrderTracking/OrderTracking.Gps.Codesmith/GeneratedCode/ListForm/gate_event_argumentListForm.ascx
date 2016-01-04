<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventargumentListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventargumentListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Botype")
					GetMessage("Argumentdescription")
					GetMessage("Valuedata")
					GetMessage("Valuetype")
					GetMessage("Localizationkey")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gateeventargument) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Gateeventargument) Container.DataItem).Argumentdescription\%>
            </td>
			<td>
                <\%# ((Gateeventargument) Container.DataItem).Valuedata\%>
            </td>
			<td>
                <\%# ((Gateeventargument) Container.DataItem).Valuetype\%>
            </td>
			<td>
                <\%# ((Gateeventargument) Container.DataItem).Localizationkey\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




