<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblLoyaltyLogForm.ascx.cs" Inherits="Loyalty.Data.TblLoyaltyLogForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTblLoyaltyLogregion fields">
				<%# GetMessage("DateTimeCreatedLabel")%><asp:TextBox ID="DateTimeCreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DateTimeCreatedErrors" runat="server" />
				<%# GetMessage("SiteIdLabel")%><asp:TextBox ID="SiteIdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SiteIdErrors" runat="server" />
				<%# GetMessage("SeverityLabel")%><asp:TextBox ID="SeverityText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SeverityErrors" runat="server" />
				<%# GetMessage("MessageLabel")%><asp:TextBox ID="MessageText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MessageErrors" runat="server" />
				<%# GetMessage("MethodLabel")%><asp:TextBox ID="MethodText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MethodErrors" runat="server" />
				<%# GetMessage("VariablesLabel")%><asp:TextBox ID="VariablesText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="VariablesErrors" runat="server" />
				<%# GetMessage("SourceLabel")%><asp:TextBox ID="SourceText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SourceErrors" runat="server" />
		</div>	
	</div>






