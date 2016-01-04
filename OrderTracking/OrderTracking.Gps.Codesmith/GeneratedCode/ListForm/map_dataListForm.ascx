<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapdataListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MapdataListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Botype")
					GetMessage("Name")
					GetMessage("Bmtilewidth")
					GetMessage("Bmtileheight")
					GetMessage("Bmtotalwidth")
					GetMessage("Bmtotalheight")
					GetMessage("Gridid")
					GetMessage("Datumid")
					GetMessage("Geomine")
					GetMessage("Geominn")
					GetMessage("Geomaxe")
					GetMessage("Geomaxn")
					GetMessage("Projtype")
					GetMessage("Projorigoe")
					GetMessage("Projorigon")
					GetMessage("Projdedx")
					GetMessage("Projdedy")
					GetMessage("Projdndx")
					GetMessage("Projdndy")
					GetMessage("Xmlfilepath")
					GetMessage("Virtualpath")
					GetMessage("Created")
					GetMessage("Projorigox")
					GetMessage("Projorigoy")
					GetMessage("Projdvde")
					GetMessage("Projdrdn")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Mapdata) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Bmtilewidth\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Bmtileheight\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Bmtotalwidth\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Bmtotalheight\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Gridid\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Datumid\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Geomine\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Geominn\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Geomaxe\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Geomaxn\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Projtype\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Projorigoe\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Projorigon\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Projdedx\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Projdedy\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Projdndx\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Projdndy\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Xmlfilepath\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Virtualpath\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Created\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Projorigox\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Projorigoy\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Projdvde\%>
            </td>
			<td>
                <\%# ((Mapdata) Container.DataItem).Projdrdn\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




