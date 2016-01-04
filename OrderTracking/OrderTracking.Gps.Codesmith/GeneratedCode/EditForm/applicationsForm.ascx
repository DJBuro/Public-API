<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ApplicationForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editApplicationregion fields">
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("MaxusersLabel")%><asp:TextBox ID="MaxusersText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxusersErrors" runat="server" />
				<%# GetMessage("ExpireLabel")%><asp:TextBox ID="ExpireText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ExpireErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
		</div>	
	</div>






