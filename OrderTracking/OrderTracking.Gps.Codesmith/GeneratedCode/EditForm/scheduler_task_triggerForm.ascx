<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SchedulertasktriggerForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SchedulertasktriggerForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editSchedulertasktriggerregion fields">
				<%# GetMessage("TriggerassemblynameLabel")%><asp:TextBox ID="TriggerassemblynameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TriggerassemblynameErrors" runat="server" />
				<%# GetMessage("TriggertypenameLabel")%><asp:TextBox ID="TriggertypenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TriggertypenameErrors" runat="server" />
				<%# GetMessage("TriggerdataLabel")%><asp:TextBox ID="TriggerdataText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TriggerdataErrors" runat="server" />
		</div>	
	</div>






