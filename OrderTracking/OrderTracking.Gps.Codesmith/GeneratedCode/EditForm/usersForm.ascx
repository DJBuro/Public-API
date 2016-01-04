<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserForm.ascx.cs" Inherits="OrderTracking.Gps.Data.UserForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editUserregion fields">
				<%# GetMessage("UsernameLabel")%><asp:TextBox ID="UsernameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UsernameErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("SurnameLabel")%><asp:TextBox ID="SurnameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SurnameErrors" runat="server" />
				<%# GetMessage("PasswordLabel")%><asp:TextBox ID="PasswordText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PasswordErrors" runat="server" />
				<%# GetMessage("EmailLabel")%><asp:TextBox ID="EmailText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EmailErrors" runat="server" />
				<%# GetMessage("ActiveLabel")%><asp:TextBox ID="ActiveText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ActiveErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("SourceaddressLabel")%><asp:TextBox ID="SourceaddressText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SourceaddressErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






