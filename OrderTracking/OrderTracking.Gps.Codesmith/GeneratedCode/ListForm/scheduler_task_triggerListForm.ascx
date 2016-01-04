<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SchedulertasktriggerListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SchedulertasktriggerListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Triggerassemblyname")
					GetMessage("Triggertypename")
					GetMessage("Triggerdata")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Schedulertasktrigger) Container.DataItem).Triggerassemblyname\%>
            </td>
			<td>
                <\%# ((Schedulertasktrigger) Container.DataItem).Triggertypename\%>
            </td>
			<td>
                <\%# ((Schedulertasktrigger) Container.DataItem).Triggerdata\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




