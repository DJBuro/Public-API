<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackerTypeListForm.ascx.cs" Inherits="OrderTracking.Data.TrackerTypeListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("GpsGateId")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((TrackerType) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((TrackerType) Container.DataItem).GpsGateId\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




