<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageproviderForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MessageproviderForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editMessageproviderregion fields">
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("MsgprovnameLabel")%><asp:TextBox ID="MsgprovnameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MsgprovnameErrors" runat="server" />
				<%# GetMessage("EnabledLabel")%><asp:TextBox ID="EnabledText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EnabledErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
				<%# GetMessage("UrlLabel")%><asp:TextBox ID="UrlText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UrlErrors" runat="server" />
				<%# GetMessage("UsernameLabel")%><asp:TextBox ID="UsernameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UsernameErrors" runat="server" />
				<%# GetMessage("PasswordLabel")%><asp:TextBox ID="PasswordText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PasswordErrors" runat="server" />
				<%# GetMessage("CallintervalLabel")%><asp:TextBox ID="CallintervalText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CallintervalErrors" runat="server" />
				<%# GetMessage("CustomlongLabel")%><asp:TextBox ID="CustomlongText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CustomlongErrors" runat="server" />
				<%# GetMessage("CustomstringLabel")%><asp:TextBox ID="CustomstringText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CustomstringErrors" runat="server" />
				<%# GetMessage("CalltimeoutLabel")%><asp:TextBox ID="CalltimeoutText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CalltimeoutErrors" runat="server" />
				<%# GetMessage("RoutelabelLabel")%><asp:TextBox ID="RoutelabelText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RoutelabelErrors" runat="server" />
				<%# GetMessage("DefaultproviderLabel")%><asp:TextBox ID="DefaultproviderText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DefaultproviderErrors" runat="server" />
		</div>	
	</div>






