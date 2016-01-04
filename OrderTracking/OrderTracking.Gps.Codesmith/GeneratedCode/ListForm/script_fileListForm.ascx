<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptfileListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ScriptfileListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Url")
					GetMessage("Language")
					GetMessage("Loadorder")
					GetMessage("Created")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Scriptfile) Container.DataItem).Url\%>
            </td>
			<td>
                <\%# ((Scriptfile) Container.DataItem).Language\%>
            </td>
			<td>
                <\%# ((Scriptfile) Container.DataItem).Loadorder\%>
            </td>
			<td>
                <\%# ((Scriptfile) Container.DataItem).Created\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




