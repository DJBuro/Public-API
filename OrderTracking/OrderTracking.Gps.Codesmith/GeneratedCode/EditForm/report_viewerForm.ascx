<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportviewerForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ReportviewerForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editReportviewerregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("ReportviewertypeLabel")%><asp:TextBox ID="ReportviewertypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ReportviewertypeErrors" runat="server" />
		</div>	
	</div>






