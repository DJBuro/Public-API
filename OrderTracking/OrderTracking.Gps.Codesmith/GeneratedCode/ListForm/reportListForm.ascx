<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ReportListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Reportname")
					GetMessage("Reportdescription")
					GetMessage("Botype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Report) Container.DataItem).Reportname\%>
            </td>
			<td>
                <\%# ((Report) Container.DataItem).Reportdescription\%>
            </td>
			<td>
                <\%# ((Report) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




