<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmsproxyForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SmsproxyForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editSmsproxyregion fields">
				<%# GetMessage("PhonenumberLabel")%><asp:TextBox ID="PhonenumberText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PhonenumberErrors" runat="server" />
				<%# GetMessage("EnabledLabel")%><asp:TextBox ID="EnabledText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EnabledErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






