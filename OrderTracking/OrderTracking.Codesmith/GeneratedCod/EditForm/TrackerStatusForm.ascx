<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackerStatusForm.ascx.cs" Inherits="OrderTracking.Data.TrackerStatusForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTrackerStatusregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
		</div>	
	</div>






