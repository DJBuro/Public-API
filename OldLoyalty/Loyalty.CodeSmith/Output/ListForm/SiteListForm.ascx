<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteListForm.ascx.cs" Inherits="Loyalty.Data.SiteListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("RamesesSiteId")
					GetMessage("Name")
					GetMessage("SiteKey")
					GetMessage("SitePassword")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Site) Container.DataItem).RamesesSiteId\%>
            </td>
			<td>
                <\%# ((Site) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((Site) Container.DataItem).SiteKey\%>
            </td>
			<td>
                <\%# ((Site) Container.DataItem).SitePassword\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




