<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeocoderproviderListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GeocoderproviderListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Typeid")
					GetMessage("Priority")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Geocoderprovider) Container.DataItem).Typeid\%>
            </td>
			<td>
                <\%# ((Geocoderprovider) Container.DataItem).Priority\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




