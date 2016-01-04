<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SessionForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SessionForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editSessionregion fields">
				<%# GetMessage("UseridLabel")%><asp:TextBox ID="UseridText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UseridErrors" runat="server" />
				<%# GetMessage("TimestampLabel")%><asp:TextBox ID="TimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampErrors" runat="server" />
				<%# GetMessage("ExpireLabel")%><asp:TextBox ID="ExpireText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ExpireErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
				<%# GetMessage("IpaddressLabel")%><asp:TextBox ID="IpaddressText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="IpaddressErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("DeviceidLabel")%><asp:TextBox ID="DeviceidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeviceidErrors" runat="server" />
		</div>	
	</div>






