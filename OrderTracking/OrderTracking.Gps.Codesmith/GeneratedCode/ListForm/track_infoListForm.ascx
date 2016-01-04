<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackinfoListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TrackinfoListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Botype")
					GetMessage("Name")
					GetMessage("Description")
					GetMessage("Minlongitude")
					GetMessage("Maxlongitude")
					GetMessage("Minlatitude")
					GetMessage("Maxlatitude")
					GetMessage("Minaltitude")
					GetMessage("Maxaltitude")
					GetMessage("Mintimestamp")
					GetMessage("Minmilliseconds")
					GetMessage("Maxtimestamp")
					GetMessage("Maxmilliseconds")
					GetMessage("Totaldistance")
					GetMessage("Deleted")
					GetMessage("Dirtycount")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Minlongitude\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Maxlongitude\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Minlatitude\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Maxlatitude\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Minaltitude\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Maxaltitude\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Mintimestamp\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Minmilliseconds\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Maxtimestamp\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Maxmilliseconds\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Totaldistance\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Deleted\%>
            </td>
			<td>
                <\%# ((Trackinfo) Container.DataItem).Dirtycount\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




