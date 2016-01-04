<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SchedulerForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SchedulerForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editSchedulerregion fields">
				<%# GetMessage("SchedulernameLabel")%><asp:TextBox ID="SchedulernameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SchedulernameErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






