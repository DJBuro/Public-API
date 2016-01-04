<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsergroupForm.ascx.cs" Inherits="OrderTracking.Gps.Data.UsergroupForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editUsergroupregion fields">
				<%# GetMessage("GrouprightidLabel")%><asp:TextBox ID="GrouprightidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GrouprightidErrors" runat="server" />
				<%# GetMessage("AdminrightidLabel")%><asp:TextBox ID="AdminrightidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AdminrightidErrors" runat="server" />
				<%# GetMessage("EnablepublictracksLabel")%><asp:TextBox ID="EnablepublictracksText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EnablepublictracksErrors" runat="server" />
		</div>	
	</div>






