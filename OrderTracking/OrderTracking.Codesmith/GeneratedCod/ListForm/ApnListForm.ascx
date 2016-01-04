<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApnListForm.ascx.cs" Inherits="OrderTracking.Data.ApnListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Provider")
					GetMessage("Name")
					GetMessage("Username")
					GetMessage("Password")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Apn) Container.DataItem).Provider\%>
            </td>
			<td>
                <\%# ((Apn) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Apn) Container.DataItem).Username\%>
            </td>
			<td>
                <\%# ((Apn) Container.DataItem).Password\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




