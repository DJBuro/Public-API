<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventexpressionForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventexpressionForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGateeventexpressionregion fields">
				<%# GetMessage("ValuedoubleLabel")%><asp:TextBox ID="ValuedoubleText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValuedoubleErrors" runat="server" />
				<%# GetMessage("ValuebooleanLabel")%><asp:TextBox ID="ValuebooleanText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValuebooleanErrors" runat="server" />
				<%# GetMessage("MindeltayfilterLabel")%><asp:TextBox ID="MindeltayfilterText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MindeltayfilterErrors" runat="server" />
				<%# GetMessage("DenominatorLabel")%><asp:TextBox ID="DenominatorText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DenominatorErrors" runat="server" />
				<%# GetMessage("RelationaloperatorLabel")%><asp:TextBox ID="RelationaloperatorText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RelationaloperatorErrors" runat="server" />
				<%# GetMessage("MathoperatorLabel")%><asp:TextBox ID="MathoperatorText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MathoperatorErrors" runat="server" />
				<%# GetMessage("ValuestringLabel")%><asp:TextBox ID="ValuestringText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValuestringErrors" runat="server" />
				<%# GetMessage("IsendexpressionLabel")%><asp:TextBox ID="IsendexpressionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="IsendexpressionErrors" runat="server" />
		</div>	
	</div>






