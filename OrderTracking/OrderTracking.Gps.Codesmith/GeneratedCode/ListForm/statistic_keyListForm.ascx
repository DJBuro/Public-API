<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatistickeyListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.StatistickeyListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Type")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Statistickey) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Statistickey) Container.DataItem).Type\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




