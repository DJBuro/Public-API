<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MsgfielddictionaryentryListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MsgfielddictionaryentryListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Multiplicator")
					GetMessage("Constant")
					GetMessage("Enabled")
					GetMessage("Savewithpos")
					GetMessage("Savechangesonly")
					GetMessage("Multiplicatorformula")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Msgfielddictionaryentry) Container.DataItem).Multiplicator\%>
            </td>
			<td>
                <\%# ((Msgfielddictionaryentry) Container.DataItem).Constant\%>
            </td>
			<td>
                <\%# ((Msgfielddictionaryentry) Container.DataItem).Enabled\%>
            </td>
			<td>
                <\%# ((Msgfielddictionaryentry) Container.DataItem).Savewithpos\%>
            </td>
			<td>
                <\%# ((Msgfielddictionaryentry) Container.DataItem).Savechangesonly\%>
            </td>
			<td>
                <\%# ((Msgfielddictionaryentry) Container.DataItem).Multiplicatorformula\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




