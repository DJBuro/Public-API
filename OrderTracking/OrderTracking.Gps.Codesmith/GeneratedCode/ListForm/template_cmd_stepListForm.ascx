<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplatecmdstepListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TemplatecmdstepListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Template")
					GetMessage("Transport")
					GetMessage("Stepdescription")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Templatecmdstep) Container.DataItem).Template\%>
            </td>
			<td>
                <\%# ((Templatecmdstep) Container.DataItem).Transport\%>
            </td>
			<td>
                <\%# ((Templatecmdstep) Container.DataItem).Stepdescription\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




