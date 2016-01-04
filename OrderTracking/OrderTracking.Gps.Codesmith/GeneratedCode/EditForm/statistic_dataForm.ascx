<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatisticdataForm.ascx.cs" Inherits="OrderTracking.Gps.Data.StatisticdataForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editStatisticdataregion fields">
				<%# GetMessage("ValueLabel")%><asp:TextBox ID="ValueText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValueErrors" runat="server" />
		</div>	
	</div>






