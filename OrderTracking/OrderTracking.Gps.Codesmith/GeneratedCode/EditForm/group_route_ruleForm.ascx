<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GrouprouteruleForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GrouprouteruleForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGrouprouteruleregion fields">
				<%# GetMessage("ServerroutelabelLabel")%><asp:TextBox ID="ServerroutelabelText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ServerroutelabelErrors" runat="server" />
				<%# GetMessage("ProviderroutelabelLabel")%><asp:TextBox ID="ProviderroutelabelText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProviderroutelabelErrors" runat="server" />
				<%# GetMessage("TransportLabel")%><asp:TextBox ID="TransportText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TransportErrors" runat="server" />
				<%# GetMessage("AutoaddLabel")%><asp:TextBox ID="AutoaddText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AutoaddErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






