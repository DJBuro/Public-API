<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptserviceListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ScriptserviceListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Url")
					GetMessage("Namespace")
					GetMessage("Method")
					GetMessage("Created")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Scriptservice) Container.DataItem).Url\%>
            </td>
			<td>
                <\%# ((Scriptservice) Container.DataItem).Namespace\%>
            </td>
			<td>
                <\%# ((Scriptservice) Container.DataItem).Method\%>
            </td>
			<td>
                <\%# ((Scriptservice) Container.DataItem).Created\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




