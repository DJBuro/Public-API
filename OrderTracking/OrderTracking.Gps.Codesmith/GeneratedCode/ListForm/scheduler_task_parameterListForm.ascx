<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SchedulertaskparameterListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SchedulertaskparameterListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Parametername")
					GetMessage("Parameterassemblyname")
					GetMessage("Parametertypename")
					GetMessage("Parameterdata")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Schedulertaskparameter) Container.DataItem).Parametername\%>
            </td>
			<td>
                <\%# ((Schedulertaskparameter) Container.DataItem).Parameterassemblyname\%>
            </td>
			<td>
                <\%# ((Schedulertaskparameter) Container.DataItem).Parametertypename\%>
            </td>
			<td>
                <\%# ((Schedulertaskparameter) Container.DataItem).Parameterdata\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




