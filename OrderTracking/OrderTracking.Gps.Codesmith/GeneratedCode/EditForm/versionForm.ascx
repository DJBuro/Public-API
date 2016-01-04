<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VersionForm.ascx.cs" Inherits="OrderTracking.Gps.Data.VersionForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editVersionregion fields">
				<%# GetMessage("ModuledescriptionLabel")%><asp:TextBox ID="ModuledescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ModuledescriptionErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
		</div>	
	</div>






