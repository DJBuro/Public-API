<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderStatusForm.ascx.cs" Inherits="OrderTracking.Data.OrderStatusForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editOrderStatusregion fields">
				<%# GetMessage("ProcessorLabel")%><asp:TextBox ID="ProcessorText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProcessorErrors" runat="server" />
				<%# GetMessage("TimeLabel")%><asp:TextBox ID="TimeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimeErrors" runat="server" />
		</div>	
	</div>






