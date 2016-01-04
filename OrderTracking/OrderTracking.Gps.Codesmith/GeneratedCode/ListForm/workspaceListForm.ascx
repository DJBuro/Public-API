<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkspaceListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.WorkspaceListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("State")
					GetMessage("Shared")
					GetMessage("Created")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Workspace) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Workspace) Container.DataItem).State\%>
            </td>
			<td>
                <\%# ((Workspace) Container.DataItem).Shared\%>
            </td>
			<td>
                <\%# ((Workspace) Container.DataItem).Created\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




