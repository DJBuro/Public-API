<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SessionListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SessionListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Userid")
					GetMessage("Timestamp")
					GetMessage("Expire")
					GetMessage("Created")
					GetMessage("Ipaddress")
					GetMessage("Botype")
					GetMessage("Deviceid")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Session) Container.DataItem).Userid\%>
            </td>
			<td>
                <\%# ((Session) Container.DataItem).Timestamp\%>
            </td>
			<td>
                <\%# ((Session) Container.DataItem).Expire\%>
            </td>
			<td>
                <\%# ((Session) Container.DataItem).Created\%>
            </td>
			<td>
                <\%# ((Session) Container.DataItem).Ipaddress\%>
            </td>
			<td>
                <\%# ((Session) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Session) Container.DataItem).Deviceid\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




