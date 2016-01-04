<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogListForm.ascx.cs" Inherits="OrderTracking.Data.LogListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("StoreId")
					GetMessage("Severity")
					GetMessage("Message")
					GetMessage("Method")
					GetMessage("Source")
					GetMessage("Created")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Log) Container.DataItem).StoreId\%>
            </td>
			<td>
                <\%# ((Log) Container.DataItem).Severity\%>
            </td>
			<td>
                <\%# ((Log) Container.DataItem).Message\%>
            </td>
			<td>
                <\%# ((Log) Container.DataItem).Method\%>
            </td>
			<td>
                <\%# ((Log) Container.DataItem).Source\%>
            </td>
			<td>
                <\%# ((Log) Container.DataItem).Created\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




