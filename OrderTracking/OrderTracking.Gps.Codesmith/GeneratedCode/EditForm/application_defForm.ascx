<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationdefForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ApplicationdefForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editApplicationdefregion fields">
				<%# GetMessage("ApplicationdefdescriptionLabel")%><asp:TextBox ID="ApplicationdefdescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ApplicationdefdescriptionErrors" runat="server" />
		</div>	
	</div>






