<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EvaluatorconditioneventdurationListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.EvaluatorconditioneventdurationListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Eventduration")
					GetMessage("Relationaloperator")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Evaluatorconditioneventduration) Container.DataItem).Eventduration\%>
            </td>
			<td>
                <\%# ((Evaluatorconditioneventduration) Container.DataItem).Relationaloperator\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




