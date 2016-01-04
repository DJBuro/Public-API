<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsergroupListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.UsergroupListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Grouprightid")
					GetMessage("Adminrightid")
					GetMessage("Enablepublictracks")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Usergroup) Container.DataItem).Grouprightid\%>
            </td>
			<td>
                <\%# ((Usergroup) Container.DataItem).Adminrightid\%>
            </td>
			<td>
                <\%# ((Usergroup) Container.DataItem).Enablepublictracks\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




