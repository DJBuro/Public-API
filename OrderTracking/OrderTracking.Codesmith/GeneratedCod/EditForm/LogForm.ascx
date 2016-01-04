<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogForm.ascx.cs" Inherits="OrderTracking.Data.LogForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editLogregion fields">
				<%# GetMessage("StoreIdLabel")%><asp:TextBox ID="StoreIdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StoreIdErrors" runat="server" />
				<%# GetMessage("SeverityLabel")%><asp:TextBox ID="SeverityText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SeverityErrors" runat="server" />
				<%# GetMessage("MessageLabel")%><asp:TextBox ID="MessageText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MessageErrors" runat="server" />
				<%# GetMessage("MethodLabel")%><asp:TextBox ID="MethodText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MethodErrors" runat="server" />
				<%# GetMessage("SourceLabel")%><asp:TextBox ID="SourceText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SourceErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
		</div>	
	</div>






