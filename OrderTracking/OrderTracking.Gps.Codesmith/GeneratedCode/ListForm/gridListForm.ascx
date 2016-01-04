<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GridListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Algorithm")
					GetMessage("Falseeasting")
					GetMessage("Falsenorthing")
					GetMessage("Origolongitude")
					GetMessage("Origolatitude")
					GetMessage("Scale")
					GetMessage("Latitudesp1")
					GetMessage("Latitudesp2")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Grid) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Grid) Container.DataItem).Algorithm\%>
            </td>
			<td>
                <\%# ((Grid) Container.DataItem).Falseeasting\%>
            </td>
			<td>
                <\%# ((Grid) Container.DataItem).Falsenorthing\%>
            </td>
			<td>
                <\%# ((Grid) Container.DataItem).Origolongitude\%>
            </td>
			<td>
                <\%# ((Grid) Container.DataItem).Origolatitude\%>
            </td>
			<td>
                <\%# ((Grid) Container.DataItem).Scale\%>
            </td>
			<td>
                <\%# ((Grid) Container.DataItem).Latitudesp1\%>
            </td>
			<td>
                <\%# ((Grid) Container.DataItem).Latitudesp2\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




