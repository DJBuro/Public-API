<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmsCredentialForm.ascx.cs" Inherits="OrderTracking.Data.SmsCredentialForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editSmsCredentialregion fields">
				<%# GetMessage("UsernameLabel")%><asp:TextBox ID="UsernameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UsernameErrors" runat="server" />
				<%# GetMessage("PasswordLabel")%><asp:TextBox ID="PasswordText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PasswordErrors" runat="server" />
				<%# GetMessage("SmsFromLabel")%><asp:TextBox ID="SmsFromText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SmsFromErrors" runat="server" />
		</div>	
	</div>






