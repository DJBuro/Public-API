<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportparameterListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ReportparameterListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Parametername")
					GetMessage("Parametertypename")
					GetMessage("Parameterassemblyname")
					GetMessage("Parameterdata")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Reportparameter) Container.DataItem).Parametername\%>
            </td>
			<td>
                <\%# ((Reportparameter) Container.DataItem).Parametertypename\%>
            </td>
			<td>
                <\%# ((Reportparameter) Container.DataItem).Parameterassemblyname\%>
            </td>
			<td>
                <\%# ((Reportparameter) Container.DataItem).Parameterdata\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




