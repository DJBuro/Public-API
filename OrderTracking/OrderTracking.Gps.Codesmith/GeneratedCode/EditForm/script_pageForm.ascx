<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptpageForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ScriptpageForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editScriptpageregion fields">
				<%# GetMessage("ApplicationbotypeLabel")%><asp:TextBox ID="ApplicationbotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ApplicationbotypeErrors" runat="server" />
				<%# GetMessage("PagenameLabel")%><asp:TextBox ID="PagenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PagenameErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
		</div>	
	</div>






