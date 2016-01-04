<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationruleListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ApplicationruleListForm" %>
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
                <\%# ((Applicationrule) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Applicationrule) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Applicationrule) Container.DataItem).Enabled\%>
            </td>
			<td>
                <\%# ((Applicationrule) Container.DataItem).Exeorder\%>
            </td>
			<td>
                <\%# ((Applicationrule) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




