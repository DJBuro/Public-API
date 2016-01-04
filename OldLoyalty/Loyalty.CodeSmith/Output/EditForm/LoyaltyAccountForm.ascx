<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoyaltyAccountForm.ascx.cs" Inherits="Loyalty.Data.LoyaltyAccountForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editLoyaltyAccountregion fields">
				<%# GetMessage("PointsLabel")%><asp:TextBox ID="PointsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PointsErrors" runat="server" />
				<%# GetMessage("DateTimeCreatedLabel")%><asp:TextBox ID="DateTimeCreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DateTimeCreatedErrors" runat="server" />
		</div>	
	</div>






