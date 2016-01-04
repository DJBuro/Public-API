<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventexpressionevaluatorListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventexpressionevaluatorListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Name")
					GetMessage("Description")
					GetMessage("Deleted")
					GetMessage("Expressionbooloperator")
					GetMessage("Created")
					GetMessage("Endexpressionbooloperator")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gateeventexpressionevaluator) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Gateeventexpressionevaluator) Container.DataItem).Description\%>
            </td>
			<td>
                <\%# ((Gateeventexpressionevaluator) Container.DataItem).Deleted\%>
            </td>
			<td>
                <\%# ((Gateeventexpressionevaluator) Container.DataItem).Expressionbooloperator\%>
            </td>
			<td>
                <\%# ((Gateeventexpressionevaluator) Container.DataItem).Created\%>
            </td>
			<td>
                <\%# ((Gateeventexpressionevaluator) Container.DataItem).Endexpressionbooloperator\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




