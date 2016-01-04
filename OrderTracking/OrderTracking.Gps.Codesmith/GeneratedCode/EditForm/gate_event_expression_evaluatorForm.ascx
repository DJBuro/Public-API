<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventexpressionevaluatorForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventexpressionevaluatorForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGateeventexpressionevaluatorregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("DeletedLabel")%><asp:TextBox ID="DeletedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeletedErrors" runat="server" />
				<%# GetMessage("ExpressionbooloperatorLabel")%><asp:TextBox ID="ExpressionbooloperatorText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ExpressionbooloperatorErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
				<%# GetMessage("EndexpressionbooloperatorLabel")%><asp:TextBox ID="EndexpressionbooloperatorText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EndexpressionbooloperatorErrors" runat="server" />
		</div>	
	</div>






