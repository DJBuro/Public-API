<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SchedulertaskListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SchedulertaskListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Taskname")
					GetMessage("Taskgroup")
					GetMessage("Execcount")
					GetMessage("Lastexectimestamp")
					GetMessage("Nextexectimestamp")
					GetMessage("State")
					GetMessage("Taskassemblyname")
					GetMessage("Tasktypename")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Schedulertask) Container.DataItem).Taskname\%>
            </td>
			<td>
                <\%# ((Schedulertask) Container.DataItem).Taskgroup\%>
            </td>
			<td>
                <\%# ((Schedulertask) Container.DataItem).Execcount\%>
            </td>
			<td>
                <\%# ((Schedulertask) Container.DataItem).Lastexectimestamp\%>
            </td>
			<td>
                <\%# ((Schedulertask) Container.DataItem).Nextexectimestamp\%>
            </td>
			<td>
                <\%# ((Schedulertask) Container.DataItem).State\%>
            </td>
			<td>
                <\%# ((Schedulertask) Container.DataItem).Taskassemblyname\%>
            </td>
			<td>
                <\%# ((Schedulertask) Container.DataItem).Tasktypename\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




