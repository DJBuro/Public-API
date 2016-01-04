<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblSiteForm.ascx.cs" Inherits="Loyalty.Data.TblSiteForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTblSiteregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("SiteKeyLabel")%><asp:TextBox ID="SiteKeyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SiteKeyErrors" runat="server" />
				<%# GetMessage("SitePasswordLabel")%><asp:TextBox ID="SitePasswordText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SitePasswordErrors" runat="server" />
		</div>	
	</div>






