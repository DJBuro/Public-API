<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GroupListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Groupname")
					GetMessage("Groupdescription")
					GetMessage("Botype")
					GetMessage("Created")
					GetMessage("Publicflag")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Group) Container.DataItem).Groupname\%>
            </td>
			<td>
                <\%# ((Group) Container.DataItem).Groupdescription\%>
            </td>
			<td>
                <\%# ((Group) Container.DataItem).Botype\%>
            </td>
			<td>
                <\%# ((Group) Container.DataItem).Created\%>
            </td>
			<td>
                <\%# ((Group) Container.DataItem).Publicflag\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




