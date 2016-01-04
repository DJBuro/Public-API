<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EvaluatorconditiondayofweekperiodForm.ascx.cs" Inherits="OrderTracking.Gps.Data.EvaluatorconditiondayofweekperiodForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editEvaluatorconditiondayofweekperiodregion fields">
				<%# GetMessage("StarttimeofdayLabel")%><asp:TextBox ID="StarttimeofdayText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StarttimeofdayErrors" runat="server" />
				<%# GetMessage("StoptimeofdayLabel")%><asp:TextBox ID="StoptimeofdayText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StoptimeofdayErrors" runat="server" />
				<%# GetMessage("DayofweekLabel")%><asp:TextBox ID="DayofweekText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DayofweekErrors" runat="server" />
				<%# GetMessage("EvaluationmethodLabel")%><asp:TextBox ID="EvaluationmethodText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EvaluationmethodErrors" runat="server" />
		</div>	
	</div>






