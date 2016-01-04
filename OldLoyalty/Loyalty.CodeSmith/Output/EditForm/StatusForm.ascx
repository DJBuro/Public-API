<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatusForm.ascx.cs" Inherits="Loyalty.Data.StatusForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editStatusregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
		</div>	
	</div>






