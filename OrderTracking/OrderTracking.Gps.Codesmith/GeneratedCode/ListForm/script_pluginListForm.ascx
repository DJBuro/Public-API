<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptpluginListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ScriptpluginListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Category")
					GetMessage("Description")
					GetMessage("Filepath")
					GetMessage("Version")
					GetMessage("Loadorder")
					GetMessage("Deleted")
					GetMessage("Created")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Scriptplugin) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Scriptplugin) Container.DataItem).Category\%>
            </td>
			<td>
                <\%# ((Scriptplugin) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Scriptplugin) Container.DataItem).Filepath\%>
            </td>
			<td>
                <\%# ((Scriptplugin) Container.DataItem).Version\%>
            </td>
			<td>
                <\%# ((Scriptplugin) Container.DataItem).Loadorder\%>
            </td>
			<td>
                <\%# ((Scriptplugin) Container.DataItem).Deleted\%>
            </td>
			<td>
                <\%# ((Scriptplugin) Container.DataItem).Created\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




