<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailnotifierListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.EmailnotifierListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Subject")
					GetMessage("Ishtml")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Emailnotifier) Container.DataItem).Subject\%>
            </td>
			<td>
                <\%# ((Emailnotifier) Container.DataItem).Ishtml\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




