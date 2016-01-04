<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackerCommandListForm.ascx.cs" Inherits="OrderTracking.Data.TrackerCommandListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Priority")
					GetMessage("Name")
					GetMessage("Command")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((TrackerCommand) Container.DataItem).Priority\%>
            </td>
			<td>
                <\%# ((TrackerCommand) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((TrackerCommand) Container.DataItem).Command\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




