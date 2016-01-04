<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EvaluatorconditioneventdurationForm.ascx.cs" Inherits="OrderTracking.Gps.Data.EvaluatorconditioneventdurationForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editEvaluatorconditioneventdurationregion fields">
				<%# GetMessage("EventdurationLabel")%><asp:TextBox ID="EventdurationText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EventdurationErrors" runat="server" />
				<%# GetMessage("RelationaloperatorLabel")%><asp:TextBox ID="RelationaloperatorText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RelationaloperatorErrors" runat="server" />
		</div>	
	</div>






