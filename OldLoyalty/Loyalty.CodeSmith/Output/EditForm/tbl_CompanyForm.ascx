<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblCompanyForm.ascx.cs" Inherits="Loyalty.Data.TblCompanyForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTblCompanyregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("RedemptionRatioLabel")%><asp:TextBox ID="RedemptionRatioText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RedemptionRatioErrors" runat="server" />
		</div>	
	</div>






