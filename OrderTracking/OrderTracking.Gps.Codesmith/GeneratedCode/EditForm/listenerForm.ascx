<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListenerForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ListenerForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editListenerregion fields">
				<%# GetMessage("EnabledLabel")%><asp:TextBox ID="EnabledText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EnabledErrors" runat="server" />
				<%# GetMessage("ServeraddressLabel")%><asp:TextBox ID="ServeraddressText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ServeraddressErrors" runat="server" />
				<%# GetMessage("ServerportLabel")%><asp:TextBox ID="ServerportText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ServerportErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






