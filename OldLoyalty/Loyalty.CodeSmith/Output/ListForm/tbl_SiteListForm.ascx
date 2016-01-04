<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblSiteListForm.ascx.cs" Inherits="Loyalty.Data.TblSiteListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
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
                <\%# ((TblSite) Container.DataItem).Name\%>
            </td>
			<td>
                <\%# ((TblSite) Container.DataItem).SiteKey\%>
            </td>
			<td>
                <\%# ((TblSite) Container.DataItem).SitePassword\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




