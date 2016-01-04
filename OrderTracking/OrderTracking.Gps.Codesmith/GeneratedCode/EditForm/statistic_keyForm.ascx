<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatistickeyForm.ascx.cs" Inherits="OrderTracking.Gps.Data.StatistickeyForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editStatistickeyregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("TypeLabel")%><asp:TextBox ID="TypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TypeErrors" runat="server" />
		</div>	
	</div>






