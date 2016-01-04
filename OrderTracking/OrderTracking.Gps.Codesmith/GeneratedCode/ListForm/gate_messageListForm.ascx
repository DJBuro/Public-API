<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GatemessageListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GatemessageListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Trackdataid")
					GetMessage("Servertimestamp")
					GetMessage("Servertimestampms")
					GetMessage("Deviceid")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gatemessage) Container.DataItem).Trackdataid\%>
            </td>
			<td>
                <\%# ((Gatemessage) Container.DataItem).Servertimestamp\%>
            </td>
			<td>
                <\%# ((Gatemessage) Container.DataItem).Servertimestampms\%>
            </td>
			<td>
                <\%# ((Gatemessage) Container.DataItem).Deviceid\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




