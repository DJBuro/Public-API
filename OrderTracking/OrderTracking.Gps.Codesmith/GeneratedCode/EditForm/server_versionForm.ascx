<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServerversionForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ServerversionForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editServerversionregion fields">
				<%# GetMessage("InstalledLabel")%><asp:TextBox ID="InstalledText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="InstalledErrors" runat="server" />
		</div>	
	</div>






