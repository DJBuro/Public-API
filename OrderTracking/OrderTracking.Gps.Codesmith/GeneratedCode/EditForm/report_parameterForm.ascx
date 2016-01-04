<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportparameterForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ReportparameterForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editReportparameterregion fields">
				<%# GetMessage("ParameternameLabel")%><asp:TextBox ID="ParameternameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ParameternameErrors" runat="server" />
				<%# GetMessage("ParametertypenameLabel")%><asp:TextBox ID="ParametertypenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ParametertypenameErrors" runat="server" />
				<%# GetMessage("ParameterassemblynameLabel")%><asp:TextBox ID="ParameterassemblynameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ParameterassemblynameErrors" runat="server" />
				<%# GetMessage("ParameterdataLabel")%><asp:TextBox ID="ParameterdataText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ParameterdataErrors" runat="server" />
		</div>	
	</div>






