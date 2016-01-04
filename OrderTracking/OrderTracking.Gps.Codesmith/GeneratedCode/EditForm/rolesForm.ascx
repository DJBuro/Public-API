<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleForm.ascx.cs" Inherits="OrderTracking.Gps.Data.RoleForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editRoleregion fields">
				<%# GetMessage("RolenameLabel")%><asp:TextBox ID="RolenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RolenameErrors" runat="server" />
				<%# GetMessage("RoledescriptionLabel")%><asp:TextBox ID="RoledescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RoledescriptionErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






