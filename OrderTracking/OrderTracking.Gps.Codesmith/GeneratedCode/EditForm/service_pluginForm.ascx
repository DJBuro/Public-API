<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServicepluginForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ServicepluginForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editServicepluginregion fields">
				<%# GetMessage("EnabledLabel")%><asp:TextBox ID="EnabledText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EnabledErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






