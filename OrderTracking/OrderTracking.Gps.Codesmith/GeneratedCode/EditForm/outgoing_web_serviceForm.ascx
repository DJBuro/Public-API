<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutgoingwebserviceForm.ascx.cs" Inherits="OrderTracking.Gps.Data.OutgoingwebserviceForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editOutgoingwebserviceregion fields">
				<%# GetMessage("NamespaceLabel")%><asp:TextBox ID="NamespaceText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NamespaceErrors" runat="server" />
				<%# GetMessage("UrlLabel")%><asp:TextBox ID="UrlText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UrlErrors" runat="server" />
				<%# GetMessage("UsernameLabel")%><asp:TextBox ID="UsernameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UsernameErrors" runat="server" />
				<%# GetMessage("PasswordLabel")%><asp:TextBox ID="PasswordText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PasswordErrors" runat="server" />
				<%# GetMessage("CallintervalLabel")%><asp:TextBox ID="CallintervalText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CallintervalErrors" runat="server" />
				<%# GetMessage("CustomlongLabel")%><asp:TextBox ID="CustomlongText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CustomlongErrors" runat="server" />
				<%# GetMessage("CustomstringLabel")%><asp:TextBox ID="CustomstringText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CustomstringErrors" runat="server" />
				<%# GetMessage("TimeoutLabel")%><asp:TextBox ID="TimeoutText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimeoutErrors" runat="server" />
		</div>	
	</div>






