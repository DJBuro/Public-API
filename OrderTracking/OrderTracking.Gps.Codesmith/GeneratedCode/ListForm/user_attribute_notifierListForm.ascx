<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserattributenotifierListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.UserattributenotifierListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Header")
					GetMessage("Type")
					GetMessage("Attributekey")
					GetMessage("Attributevaluestart")
					GetMessage("Attributevalueend")
					GetMessage("Applicationid")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Userattributenotifier) Container.DataItem).Header\%>
            </td>
			<td>
                <\%# ((Userattributenotifier) Container.DataItem).Type\%>
            </td>
			<td>
                <\%# ((Userattributenotifier) Container.DataItem).Attributekey\%>
            </td>
			<td>
                <\%# ((Userattributenotifier) Container.DataItem).Attributevaluestart\%>
            </td>
			<td>
                <\%# ((Userattributenotifier) Container.DataItem).Attributevalueend\%>
            </td>
			<td>
                <\%# ((Userattributenotifier) Container.DataItem).Applicationid\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




