<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblCountryListForm.ascx.cs" Inherits="Loyalty.Data.TblCountryListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("ISOCode")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((TblCountry) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((TblCountry) Container.DataItem).ISOCode\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




