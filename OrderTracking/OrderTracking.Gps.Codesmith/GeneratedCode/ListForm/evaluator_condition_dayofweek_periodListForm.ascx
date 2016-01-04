<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EvaluatorconditiondayofweekperiodListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.EvaluatorconditiondayofweekperiodListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Starttimeofday")
					GetMessage("Stoptimeofday")
					GetMessage("Dayofweek")
					GetMessage("Evaluationmethod")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Evaluatorconditiondayofweekperiod) Container.DataItem).Starttimeofday\%>
            </td>
			<td>
                <\%# ((Evaluatorconditiondayofweekperiod) Container.DataItem).Stoptimeofday\%>
            </td>
			<td>
                <\%# ((Evaluatorconditiondayofweekperiod) Container.DataItem).Dayofweek\%>
            </td>
			<td>
                <\%# ((Evaluatorconditiondayofweekperiod) Container.DataItem).Evaluationmethod\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




