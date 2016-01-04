<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventexpressionListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventexpressionListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Valuedouble")
					GetMessage("Valueboolean")
					GetMessage("Mindeltayfilter")
					GetMessage("Denominator")
					GetMessage("Relationaloperator")
					GetMessage("Mathoperator")
					GetMessage("Valuestring")
					GetMessage("Isendexpression")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gateeventexpression) Container.DataItem).Valuedouble\%>
            </td>
			<td>
                <\%# ((Gateeventexpression) Container.DataItem).Valueboolean\%>
            </td>
			<td>
                <\%# ((Gateeventexpression) Container.DataItem).Mindeltayfilter\%>
            </td>
			<td>
                <\%# ((Gateeventexpression) Container.DataItem).Denominator\%>
            </td>
			<td>
                <\%# ((Gateeventexpression) Container.DataItem).Relationaloperator\%>
            </td>
			<td>
                <\%# ((Gateeventexpression) Container.DataItem).Mathoperator\%>
            </td>
			<td>
                <\%# ((Gateeventexpression) Container.DataItem).Valuestring\%>
            </td>
			<td>
                <\%# ((Gateeventexpression) Container.DataItem).Isendexpression\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




