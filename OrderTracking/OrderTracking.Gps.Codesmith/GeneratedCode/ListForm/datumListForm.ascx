<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatumListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.DatumListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Semimajoraxis")
					GetMessage("E2")
					GetMessage("Deltax")
					GetMessage("Deltay")
					GetMessage("Deltaz")
					GetMessage("Rotx")
					GetMessage("Roty")
					GetMessage("Rotz")
					GetMessage("Scale")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Datum) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Datum) Container.DataItem).Semimajoraxis\%>
            </td>
			<td>
                <\%# ((Datum) Container.DataItem).E2\%>
            </td>
			<td>
                <\%# ((Datum) Container.DataItem).Deltax\%>
            </td>
			<td>
                <\%# ((Datum) Container.DataItem).Deltay\%>
            </td>
			<td>
                <\%# ((Datum) Container.DataItem).Deltaz\%>
            </td>
			<td>
                <\%# ((Datum) Container.DataItem).Rotx\%>
            </td>
			<td>
                <\%# ((Datum) Container.DataItem).Roty\%>
            </td>
			<td>
                <\%# ((Datum) Container.DataItem).Rotz\%>
            </td>
			<td>
                <\%# ((Datum) Container.DataItem).Scale\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




