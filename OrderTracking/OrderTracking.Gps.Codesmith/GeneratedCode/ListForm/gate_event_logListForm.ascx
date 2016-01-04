<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventlogListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventlogListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Modifiedbyuserid")
					GetMessage("Servertimestamp")
					GetMessage("Servertimestampms")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gateeventlog) Container.DataItem).Modifiedbyuserid\%>
            </td>
			<td>
                <\%# ((Gateeventlog) Container.DataItem).Servertimestamp\%>
            </td>
			<td>
                <\%# ((Gateeventlog) Container.DataItem).Servertimestampms\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




