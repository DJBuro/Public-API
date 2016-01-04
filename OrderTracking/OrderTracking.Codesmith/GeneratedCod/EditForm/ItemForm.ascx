<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemForm.ascx.cs" Inherits="OrderTracking.Data.ItemForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editItemregion fields">
				<%# GetMessage("QuantityLabel")%><asp:TextBox ID="QuantityText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="QuantityErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
		</div>	
	</div>






