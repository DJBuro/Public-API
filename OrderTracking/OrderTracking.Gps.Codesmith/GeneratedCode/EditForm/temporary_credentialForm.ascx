<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemporarycredentialForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TemporarycredentialForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTemporarycredentialregion fields">
				<%# GetMessage("ExpireLabel")%><asp:TextBox ID="ExpireText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ExpireErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






