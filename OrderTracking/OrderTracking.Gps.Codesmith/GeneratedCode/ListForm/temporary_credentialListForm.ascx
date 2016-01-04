<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemporarycredentialListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TemporarycredentialListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Expire")
					GetMessage("Botype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Temporarycredential) Container.DataItem).Expire\%>
            </td>
			<td>
                <\%# ((Temporarycredential) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




