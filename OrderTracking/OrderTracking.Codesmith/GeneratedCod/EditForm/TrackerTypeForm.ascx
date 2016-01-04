<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackerTypeForm.ascx.cs" Inherits="OrderTracking.Data.TrackerTypeForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTrackerTyperegion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("GpsGateIdLabel")%><asp:TextBox ID="GpsGateIdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GpsGateIdErrors" runat="server" />
		</div>	
	</div>






