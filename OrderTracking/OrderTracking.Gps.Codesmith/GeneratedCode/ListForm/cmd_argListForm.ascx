<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmdargListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.CmdargListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Sentence")
					GetMessage("Sentenceindex")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Cmdarg) Container.DataItem).Sentence\%>
            </td>
			<td>
                <\%# ((Cmdarg) Container.DataItem).Sentenceindex\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




