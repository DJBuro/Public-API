<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderItemHistoryForm.ascx.cs" Inherits="Loyalty.Data.OrderItemHistoryForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editOrderItemHistoryregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("ItemPriceLabel")%><asp:TextBox ID="ItemPriceText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ItemPriceErrors" runat="server" />
				<%# GetMessage("ItemLoyaltyPointsLabel")%><asp:TextBox ID="ItemLoyaltyPointsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ItemLoyaltyPointsErrors" runat="server" />
		</div>	
	</div>






