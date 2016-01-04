<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessagetemplateForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MessagetemplateForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editMessagetemplateregion fields">
				<%# GetMessage("TemplatetextLabel")%><asp:TextBox ID="TemplatetextText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TemplatetextErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






