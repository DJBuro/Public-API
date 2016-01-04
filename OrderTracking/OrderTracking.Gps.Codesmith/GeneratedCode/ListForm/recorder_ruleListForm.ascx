<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecorderruleListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.RecorderruleListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Description")
					GetMessage("Enabled")
					GetMessage("Exeorder")
					GetMessage("Botype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Recorderrule) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Recorderrule) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Recorderrule) Container.DataItem).Enabled\%>
            </td>
			<td>
                <\%# ((Recorderrule) Container.DataItem).Exeorder\%>
            </td>
			<td>
                <\%# ((Recorderrule) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




