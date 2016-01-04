<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblLoyaltyUserForm.ascx.cs" Inherits="Loyalty.Data.TblLoyaltyUserForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTblLoyaltyUserregion fields">
				<%# GetMessage("FirstNameLabel")%><asp:TextBox ID="FirstNameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="FirstNameErrors" runat="server" />
				<%# GetMessage("MiddleInitialLabel")%><asp:TextBox ID="MiddleInitialText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MiddleInitialErrors" runat="server" />
				<%# GetMessage("SurNameLabel")%><asp:TextBox ID="SurNameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SurNameErrors" runat="server" />
				<%# GetMessage("DateTimeCreatedLabel")%><asp:TextBox ID="DateTimeCreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DateTimeCreatedErrors" runat="server" />
				<%# GetMessage("EmailAddressLabel")%><asp:TextBox ID="EmailAddressText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EmailAddressErrors" runat="server" />
				<%# GetMessage("PasswordLabel")%><asp:TextBox ID="PasswordText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PasswordErrors" runat="server" />
		</div>	
	</div>






