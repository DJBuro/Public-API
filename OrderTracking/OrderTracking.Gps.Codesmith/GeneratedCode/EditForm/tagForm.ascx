<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TagForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTagregion fields">
				<%# GetMessage("TagnameLabel")%><asp:TextBox ID="TagnameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TagnameErrors" runat="server" />
				<%# GetMessage("TagdescriptionLabel")%><asp:TextBox ID="TagdescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TagdescriptionErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






