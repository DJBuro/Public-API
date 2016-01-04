<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkspaceForm.ascx.cs" Inherits="OrderTracking.Gps.Data.WorkspaceForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editWorkspaceregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("StateLabel")%><asp:TextBox ID="StateText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StateErrors" runat="server" />
				<%# GetMessage("SharedLabel")%><asp:TextBox ID="SharedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SharedErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
		</div>	
	</div>






