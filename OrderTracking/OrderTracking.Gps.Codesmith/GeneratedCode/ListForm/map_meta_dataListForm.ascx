<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapmetadataListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MapmetadataListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Value")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Mapmetadata) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Mapmetadata) Container.DataItem).Value\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




