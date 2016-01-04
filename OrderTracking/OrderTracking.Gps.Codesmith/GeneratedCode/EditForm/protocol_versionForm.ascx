<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProtocolversionForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ProtocolversionForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editProtocolversionregion fields">
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("ProtocolidLabel")%><asp:TextBox ID="ProtocolidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProtocolidErrors" runat="server" />
				<%# GetMessage("MajorversionLabel")%><asp:TextBox ID="MajorversionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MajorversionErrors" runat="server" />
				<%# GetMessage("MinorversionLabel")%><asp:TextBox ID="MinorversionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinorversionErrors" runat="server" />
				<%# GetMessage("ClientnameLabel")%><asp:TextBox ID="ClientnameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ClientnameErrors" runat="server" />
		</div>	
	</div>






