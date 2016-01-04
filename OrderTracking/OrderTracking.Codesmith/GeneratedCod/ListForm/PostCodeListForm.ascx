<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostCodeListForm.ascx.cs" Inherits="OrderTracking.Data.PostCodeListForm" %>
<Cacd:ValidationSummary ID="validationSummary" Provider="summary" runat="server" />
<asp:Repeater ID="dataItemsList" runat="server">
    <HeaderTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="suggestedTable">
            <thead>
				<tr>
					GetMessage("PostCode")
					GetMessage("Del1")
					GetMessage("Longitude")
					GetMessage("Latitude")
					GetMessage("Del2")
					GetMessage("Del3")
					GetMessage("Del4")
					GetMessage("Del5")
					GetMessage("Del6")
					GetMessage("Del7")
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
		<tr>
			<td>
                <\%# ((PostCode) Container.DataItem).PostCode\%>
            </td>
			<td>
                <\%# ((PostCode) Container.DataItem).Del1\%>
            </td>
			<td>
                <\%# ((PostCode) Container.DataItem).Longitude\%>
            </td>
			<td>
                <\%# ((PostCode) Container.DataItem).Latitude\%>
            </td>
			<td>
                <\%# ((PostCode) Container.DataItem).Del2\%>
            </td>
			<td>
                <\%# ((PostCode) Container.DataItem).Del3\%>
            </td>
			<td>
                <\%# ((PostCode) Container.DataItem).Del4\%>
            </td>
			<td>
                <\%# ((PostCode) Container.DataItem).Del5\%>
            </td>
			<td>
                <\%# ((PostCode) Container.DataItem).Del6\%>
            </td>
			<td>
                <\%# ((PostCode) Container.DataItem).Del7\%>
            </td>
		</tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody></table>
    </FooterTemplate>
</asp:Repeater>




