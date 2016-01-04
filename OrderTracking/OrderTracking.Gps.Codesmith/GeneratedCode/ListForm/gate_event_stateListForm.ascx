<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventstateListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventstateListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Description")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gateeventstate) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Gateeventstate) Container.DataItem).Description\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




