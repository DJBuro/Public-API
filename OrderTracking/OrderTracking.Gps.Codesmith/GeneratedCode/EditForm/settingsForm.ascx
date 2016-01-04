<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SettingForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editSettingregion fields">
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("NamespaceLabel")%><asp:TextBox ID="NamespaceText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NamespaceErrors" runat="server" />
				<%# GetMessage("ValuenameLabel")%><asp:TextBox ID="ValuenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValuenameErrors" runat="server" />
				<%# GetMessage("ValuetypeLabel")%><asp:TextBox ID="ValuetypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValuetypeErrors" runat="server" />
				<%# GetMessage("ValuedataLabel")%><asp:TextBox ID="ValuedataText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValuedataErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
		</div>	
	</div>






