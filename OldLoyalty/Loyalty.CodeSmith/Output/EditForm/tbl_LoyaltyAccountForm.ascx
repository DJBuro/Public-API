<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblLoyaltyAccountForm.ascx.cs" Inherits="Loyalty.Data.TblLoyaltyAccountForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTblLoyaltyAccountregion fields">
				<%# GetMessage("PointsLabel")%><asp:TextBox ID="PointsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PointsErrors" runat="server" />
				<%# GetMessage("DateTimeCreatedLabel")%><asp:TextBox ID="DateTimeCreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DateTimeCreatedErrors" runat="server" />
		</div>	
	</div>






