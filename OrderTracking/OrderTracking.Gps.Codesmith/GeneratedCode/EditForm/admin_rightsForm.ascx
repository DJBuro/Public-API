<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminrightForm.ascx.cs" Inherits="OrderTracking.Gps.Data.AdminrightForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editAdminrightregion fields">
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






