<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackinfomodListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TrackinfomodListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Trackinfoid")
					GetMessage("Name")
					GetMessage("Description")
					GetMessage("Ownerid")
					GetMessage("Trackcategoryid")
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
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Trackinfoid\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Ownerid\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Trackcategoryid\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Minlongitude\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Maxlongitude\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Minlatitude\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Maxlatitude\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Minaltitude\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Maxaltitude\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Mintimestamp\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Minmilliseconds\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Maxtimestamp\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Maxmilliseconds\%>
            </td>
			<td>
                <\%# ((Trackinfomod) Container.DataItem).Totaldistance\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




