<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoordinateListForm.ascx.cs" Inherits="OrderTracking.Data.CoordinateListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Longitude")
					GetMessage("Latitude")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Coordinate) Container.DataItem).Longitude\%>
            </td>
			<td>
                <\%# ((Coordinate) Container.DataItem).Latitude\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




