<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobilenetworkForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MobilenetworkForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editMobilenetworkregion fields">
				<%# GetMessage("OperatorLabel")%><asp:TextBox ID="OperatorText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="OperatorErrors" runat="server" />
				<%# GetMessage("UsernameLabel")%><asp:TextBox ID="UsernameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UsernameErrors" runat="server" />
				<%# GetMessage("PasswordLabel")%><asp:TextBox ID="PasswordText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PasswordErrors" runat="server" />
				<%# GetMessage("ApnLabel")%><asp:TextBox ID="ApnText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ApnErrors" runat="server" />
				<%# GetMessage("Dns1Label")%><asp:TextBox ID="Dns1Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Dns1Errors" runat="server" />
				<%# GetMessage("Dns2Label")%><asp:TextBox ID="Dns2Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Dns2Errors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
		</div>	
	</div>






