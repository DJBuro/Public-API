<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmdargForm.ascx.cs" Inherits="OrderTracking.Gps.Data.CmdargForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editCmdargregion fields">
				<%# GetMessage("SentenceLabel")%><asp:TextBox ID="SentenceText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SentenceErrors" runat="server" />
				<%# GetMessage("SentenceindexLabel")%><asp:TextBox ID="SentenceindexText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SentenceindexErrors" runat="server" />
		</div>	
	</div>






