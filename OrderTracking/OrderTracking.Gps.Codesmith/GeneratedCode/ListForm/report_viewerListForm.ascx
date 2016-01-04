<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportviewerListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ReportviewerListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Description")
					GetMessage("Reportviewertype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Reportviewer) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Reportviewer) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Reportviewer) Container.DataItem).Reportviewertype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




