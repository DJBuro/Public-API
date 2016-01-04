<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupreferrerlogListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GroupreferrerlogListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Refurl")
					GetMessage("Hits")
					GetMessage("Timestamp")
					GetMessage("Created")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Groupreferrerlog) Container.DataItem).Refurl\%>
            </td>
			<td>
                <\%# ((Groupreferrerlog) Container.DataItem).Hits\%>
            </td>
			<td>
                <\%# ((Groupreferrerlog) Container.DataItem).Timestamp\%>
            </td>
			<td>
                <\%# ((Groupreferrerlog) Container.DataItem).Created\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




