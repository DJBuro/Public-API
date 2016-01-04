<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApptemplateListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ApptemplateListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Botype")
					GetMessage("Name")
					GetMessage("Description")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Apptemplate) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Apptemplate) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Apptemplate) Container.DataItem).Description\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




