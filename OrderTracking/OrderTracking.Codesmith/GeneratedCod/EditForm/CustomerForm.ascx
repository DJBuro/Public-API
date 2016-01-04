<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerForm.ascx.cs" Inherits="OrderTracking.Data.CustomerForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editCustomerregion fields">
				<%# GetMessage("ExternalIdLabel")%><asp:TextBox ID="ExternalIdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ExternalIdErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("CredentialsLabel")%><asp:TextBox ID="CredentialsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CredentialsErrors" runat="server" />
		</div>	
	</div>






