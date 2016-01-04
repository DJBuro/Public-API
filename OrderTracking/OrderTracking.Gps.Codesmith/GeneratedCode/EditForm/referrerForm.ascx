<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReferrerForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ReferrerForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editReferrerregion fields">
				<%# GetMessage("RefurlLabel")%><asp:TextBox ID="RefurlText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RefurlErrors" runat="server" />
		</div>	
	</div>






