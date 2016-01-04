<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackerCommandForm.ascx.cs" Inherits="OrderTracking.Data.TrackerCommandForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTrackerCommandregion fields">
				<%# GetMessage("PriorityLabel")%><asp:TextBox ID="PriorityText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PriorityErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("CommandLabel")%><asp:TextBox ID="CommandText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CommandErrors" runat="server" />
		</div>	
	</div>






