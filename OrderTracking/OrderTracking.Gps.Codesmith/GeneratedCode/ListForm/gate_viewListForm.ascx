<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateviewListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateviewListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Viewname")
					GetMessage("Viewdescription")
					GetMessage("Statusfilter")
					GetMessage("Matchalltags")
					GetMessage("Botype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Gateview) Container.DataItem).Viewname\%>
            </td>
			<td>
                <\%# ((Gateview) Container.DataItem).Viewdescription\%>
            </td>
			<td>
                <\%# ((Gateview) Container.DataItem).Statusfilter\%>
            </td>
			<td>
                <\%# ((Gateview) Container.DataItem).Matchalltags\%>
            </td>
			<td>
                <\%# ((Gateview) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




