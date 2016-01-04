<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmsproxyqueueForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SmsproxyqueueForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editSmsproxyqueueregion fields">
				<%# GetMessage("SmsproxyidLabel")%><asp:TextBox ID="SmsproxyidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SmsproxyidErrors" runat="server" />
		</div>	
	</div>






