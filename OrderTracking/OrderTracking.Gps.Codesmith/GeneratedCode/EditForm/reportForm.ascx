<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ReportForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editReportregion fields">
				<%# GetMessage("ReportnameLabel")%><asp:TextBox ID="ReportnameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ReportnameErrors" runat="server" />
				<%# GetMessage("ReportdescriptionLabel")%><asp:TextBox ID="ReportdescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ReportdescriptionErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






