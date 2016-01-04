<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaguserListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TaguserListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Servertimestamp")
					GetMessage("Servertimestampms")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Taguser) Container.DataItem).Servertimestamp\%>
            </td>
			<td>
                <\%# ((Taguser) Container.DataItem).Servertimestampms\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




