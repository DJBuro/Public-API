<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TagListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Tagname")
					GetMessage("Tagdescription")
					GetMessage("Botype")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Tag) Container.DataItem).Tagname\%>
            </td>
			<td>
                <\%# ((Tag) Container.DataItem).Tagdescription\%>
            </td>
			<td>
                <\%# ((Tag) Container.DataItem).Botype\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




