<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GrouprouteruleListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GrouprouteruleListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Serverroutelabel")
					GetMessage("Providerroutelabel")
					GetMessage("Transport")
					GetMessage("Autoadd")
					GetMessage("Botype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Grouprouterule) Container.DataItem).Serverroutelabel\%>
            </td>
			<td>
                <\%# ((Grouprouterule) Container.DataItem).Providerroutelabel\%>
            </td>
			<td>
                <\%# ((Grouprouterule) Container.DataItem).Transport\%>
            </td>
			<td>
                <\%# ((Grouprouterule) Container.DataItem).Autoadd\%>
            </td>
			<td>
                <\%# ((Grouprouterule) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




