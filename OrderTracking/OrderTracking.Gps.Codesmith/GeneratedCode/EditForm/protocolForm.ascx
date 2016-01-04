<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProtocolForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ProtocolForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editProtocolregion fields">
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("AdapterbotypeLabel")%><asp:TextBox ID="AdapterbotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AdapterbotypeErrors" runat="server" />
		</div>	
	</div>






