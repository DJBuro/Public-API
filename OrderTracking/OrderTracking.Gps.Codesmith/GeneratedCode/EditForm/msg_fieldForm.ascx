<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MsgfieldForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MsgfieldForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editMsgfieldregion fields">
				<%# GetMessage("TypeLabel")%><asp:TextBox ID="TypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TypeErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("LocalizationkeyLabel")%><asp:TextBox ID="LocalizationkeyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LocalizationkeyErrors" runat="server" />
		</div>	
	</div>






