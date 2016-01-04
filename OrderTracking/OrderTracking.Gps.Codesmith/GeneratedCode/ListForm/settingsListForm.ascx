<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SettingListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Botype")
					GetMessage("Namespace")
					GetMessage("Valuename")
					GetMessage("Valuetype")
					GetMessage("Valuedata")
					GetMessage("Description")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Setting) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Setting) Container.DataItem).Namespace\%>
            </td>
			<td>
                <\%# ((Setting) Container.DataItem).Valuename\%>
            </td>
			<td>
                <\%# ((Setting) Container.DataItem).Valuetype\%>
            </td>
			<td>
                <\%# ((Setting) Container.DataItem).Valuedata\%>
            </td>
			<td>
                <\%# ((Setting) Container.DataItem).Description\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




