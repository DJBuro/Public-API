<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LicenseForm.ascx.cs" Inherits="OrderTracking.Gps.Data.LicenseForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editLicenseregion fields">
				<%# GetMessage("LicensedusersLabel")%><asp:TextBox ID="LicensedusersText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LicensedusersErrors" runat="server" />
				<%# GetMessage("LicenseidLabel")%><asp:TextBox ID="LicenseidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LicenseidErrors" runat="server" />
				<%# GetMessage("CustomeridLabel")%><asp:TextBox ID="CustomeridText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CustomeridErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("SignatureLabel")%><asp:TextBox ID="SignatureText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SignatureErrors" runat="server" />
				<%# GetMessage("EmailLabel")%><asp:TextBox ID="EmailText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EmailErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






