<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyForm.ascx.cs" Inherits="Loyalty.Data.CompanyForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editCompanyregion fields">
				<%# GetMessage("RamesesHeadOfficeIdLabel")%><asp:TextBox ID="RamesesHeadOfficeIdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RamesesHeadOfficeIdErrors" runat="server" />
				<%# GetMessage("RamesesCompanyIdLabel")%><asp:TextBox ID="RamesesCompanyIdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RamesesCompanyIdErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("RedemptionRatioLabel")%><asp:TextBox ID="RedemptionRatioText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RedemptionRatioErrors" runat="server" />
				<%# GetMessage("CompanyKeyLabel")%><asp:TextBox ID="CompanyKeyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CompanyKeyErrors" runat="server" />
				<%# GetMessage("CompanyPasswordLabel")%><asp:TextBox ID="CompanyPasswordText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CompanyPasswordErrors" runat="server" />
		</div>	
	</div>






