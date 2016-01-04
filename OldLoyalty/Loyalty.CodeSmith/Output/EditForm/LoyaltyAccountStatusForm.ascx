<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoyaltyAccountStatusForm.ascx.cs" Inherits="Loyalty.Data.LoyaltyAccountStatusForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editLoyaltyAccountStatusregion fields">
				<%# GetMessage("ReasonLabel")%><asp:TextBox ID="ReasonText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ReasonErrors" runat="server" />
				<%# GetMessage("DateTimeCreatedLabel")%><asp:TextBox ID="DateTimeCreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DateTimeCreatedErrors" runat="server" />
		</div>	
	</div>






