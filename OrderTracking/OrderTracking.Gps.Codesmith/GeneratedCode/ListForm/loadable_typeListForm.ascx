<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadabletypeListForm.ascx.cs" Inherits="OrderTracking.Gps.Data.LoadabletypeListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("Assemblyname")
					GetMessage("Typename")
					GetMessage("Typedescription")
					GetMessage("Basetypename")
					GetMessage("Basetypedescription")
					GetMessage("Deleted")
					GetMessage("Created")
					GetMessage("Version")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((Loadabletype) Container.DataItem).Assemblyname\%>
            </td>
			<td>
                <\%# ((Loadabletype) Container.DataItem).Typename\%>
            </td>
			<td>
                <\%# ((Loadabletype) Container.DataItem).Typedescription\%>
            </td>
			<td>
                <\%# ((Loadabletype) Container.DataItem).Basetypename\%>
            </td>
			<td>
                <\%# ((Loadabletype) Container.DataItem).Basetypedescription\%>
            </td>
			<td>
                <\%# ((Loadabletype) Container.DataItem).Deleted\%>
            </td>
			<td>
                <\%# ((Loadabletype) Container.DataItem).Created\%>
            </td>
			<td>
                <\%# ((Loadabletype) Container.DataItem).Version\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




