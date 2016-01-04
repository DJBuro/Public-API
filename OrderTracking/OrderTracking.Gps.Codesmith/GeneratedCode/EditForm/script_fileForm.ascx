<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptfileForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ScriptfileForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editScriptfileregion fields">
				<%# GetMessage("UrlLabel")%><asp:TextBox ID="UrlText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UrlErrors" runat="server" />
				<%# GetMessage("LanguageLabel")%><asp:TextBox ID="LanguageText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LanguageErrors" runat="server" />
				<%# GetMessage("LoadorderLabel")%><asp:TextBox ID="LoadorderText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LoadorderErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
		</div>	
	</div>






