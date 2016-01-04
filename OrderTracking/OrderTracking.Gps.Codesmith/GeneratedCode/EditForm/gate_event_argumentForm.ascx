<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventargumentForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventargumentForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGateeventargumentregion fields">
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("ArgumentdescriptionLabel")%><asp:TextBox ID="ArgumentdescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ArgumentdescriptionErrors" runat="server" />
				<%# GetMessage("ValuedataLabel")%><asp:TextBox ID="ValuedataText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValuedataErrors" runat="server" />
				<%# GetMessage("ValuetypeLabel")%><asp:TextBox ID="ValuetypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValuetypeErrors" runat="server" />
				<%# GetMessage("LocalizationkeyLabel")%><asp:TextBox ID="LocalizationkeyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LocalizationkeyErrors" runat="server" />
		</div>	
	</div>






