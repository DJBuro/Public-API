<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptserviceForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ScriptserviceForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editScriptserviceregion fields">
				<%# GetMessage("UrlLabel")%><asp:TextBox ID="UrlText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UrlErrors" runat="server" />
				<%# GetMessage("NamespaceLabel")%><asp:TextBox ID="NamespaceText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NamespaceErrors" runat="server" />
				<%# GetMessage("MethodLabel")%><asp:TextBox ID="MethodText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MethodErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
		</div>	
	</div>






