<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UicontrolstateForm.ascx.cs" Inherits="OrderTracking.Gps.Data.UicontrolstateForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editUicontrolstateregion fields">
				<%# GetMessage("StateLabel")%><asp:TextBox ID="StateText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StateErrors" runat="server" />
		</div>	
	</div>






