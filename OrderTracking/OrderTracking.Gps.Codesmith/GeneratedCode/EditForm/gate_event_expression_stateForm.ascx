<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventexpressionstateForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventexpressionstateForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGateeventexpressionstateregion fields">
				<%# GetMessage("CustomstateLabel")%><asp:TextBox ID="CustomstateText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CustomstateErrors" runat="server" />
		</div>	
	</div>






