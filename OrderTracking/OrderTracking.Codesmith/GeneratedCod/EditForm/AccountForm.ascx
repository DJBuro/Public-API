<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountForm.ascx.cs" Inherits="OrderTracking.Data.AccountForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editAccountregion fields">
				<%# GetMessage("UserNameLabel")%><asp:TextBox ID="UserNameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UserNameErrors" runat="server" />
				<%# GetMessage("PasswordLabel")%><asp:TextBox ID="PasswordText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PasswordErrors" runat="server" />
				<%# GetMessage("GpsEnabledLabel")%><asp:TextBox ID="GpsEnabledText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GpsEnabledErrors" runat="server" />
		</div>	
	</div>






