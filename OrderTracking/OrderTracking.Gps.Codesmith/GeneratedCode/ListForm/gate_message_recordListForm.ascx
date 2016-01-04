<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GatemessagerecordListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GatemessagerecordListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Databool")
					GetMessage("Dataint")
					GetMessage("Datadouble")
					GetMessage("Datatext")
					GetMessage("Datalongtext")
					GetMessage("Datadatetime")
					GetMessage("Savechangesonly")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gatemessagerecord) Container.DataItem).Databool\%>
            </td>
			<td>
                <\%# ((Gatemessagerecord) Container.DataItem).Dataint\%>
            </td>
			<td>
                <\%# ((Gatemessagerecord) Container.DataItem).Datadouble\%>
            </td>
			<td>
                <\%# ((Gatemessagerecord) Container.DataItem).Datatext\%>
            </td>
			<td>
                <\%# ((Gatemessagerecord) Container.DataItem).Datalongtext\%>
            </td>
			<td>
                <\%# ((Gatemessagerecord) Container.DataItem).Datadatetime\%>
            </td>
			<td>
                <\%# ((Gatemessagerecord) Container.DataItem).Savechangesonly\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




