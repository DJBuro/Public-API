<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApnForm.ascx.cs" Inherits="OrderTracking.Data.ApnForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editApnregion fields">
				<%# GetMessage("ProviderLabel")%><asp:TextBox ID="ProviderText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProviderErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("UsernameLabel")%><asp:TextBox ID="UsernameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UsernameErrors" runat="server" />
				<%# GetMessage("PasswordLabel")%><asp:TextBox ID="PasswordText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PasswordErrors" runat="server" />
		</div>	
	</div>






