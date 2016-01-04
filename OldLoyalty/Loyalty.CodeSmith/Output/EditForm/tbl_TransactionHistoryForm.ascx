<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblTransactionHistoryForm.ascx.cs" Inherits="Loyalty.Data.TblTransactionHistoryForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTblTransactionHistoryregion fields">
				<%# GetMessage("DateTimeOrderedLabel")%><asp:TextBox ID="DateTimeOrderedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DateTimeOrderedErrors" runat="server" />
				<%# GetMessage("OrderIdLabel")%><asp:TextBox ID="OrderIdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="OrderIdErrors" runat="server" />
				<%# GetMessage("OrderTypeIdLabel")%><asp:TextBox ID="OrderTypeIdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="OrderTypeIdErrors" runat="server" />
				<%# GetMessage("LoyaltyPointsRedeemedLabel")%><asp:TextBox ID="LoyaltyPointsRedeemedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LoyaltyPointsRedeemedErrors" runat="server" />
				<%# GetMessage("LoyaltyPointsAddedLabel")%><asp:TextBox ID="LoyaltyPointsAddedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LoyaltyPointsAddedErrors" runat="server" />
				<%# GetMessage("LoyaltyPointsValueLabel")%><asp:TextBox ID="LoyaltyPointsValueText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LoyaltyPointsValueErrors" runat="server" />
				<%# GetMessage("OrderTotalLabel")%><asp:TextBox ID="OrderTotalText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="OrderTotalErrors" runat="server" />
		</div>	
	</div>






