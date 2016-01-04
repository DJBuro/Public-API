<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptpageListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ScriptpageListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Applicationbotype")
					GetMessage("Pagename")
					GetMessage("Created")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Scriptpage) Container.DataItem).Applicationbotype\%>
            </td>
			<td>
                <\%# ((Scriptpage) Container.DataItem).Pagename\%>
            </td>
			<td>
                <\%# ((Scriptpage) Container.DataItem).Created\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




