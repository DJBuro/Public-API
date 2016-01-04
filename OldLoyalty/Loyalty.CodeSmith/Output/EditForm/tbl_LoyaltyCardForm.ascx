<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblLoyaltyCardForm.ascx.cs" Inherits="Loyalty.Data.TblLoyaltyCardForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTblLoyaltyCardregion fields">
				<%# GetMessage("CardNumberLabel")%><asp:TextBox ID="CardNumberText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CardNumberErrors" runat="server" />
				<%# GetMessage("DateTimeCreatedLabel")%><asp:TextBox ID="DateTimeCreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DateTimeCreatedErrors" runat="server" />
				<%# GetMessage("PinLabel")%><asp:TextBox ID="PinText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PinErrors" runat="server" />
		</div>	
	</div>






