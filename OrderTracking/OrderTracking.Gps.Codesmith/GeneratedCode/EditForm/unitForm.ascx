<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UnitForm.ascx.cs" Inherits="OrderTracking.Gps.Data.UnitForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editUnitregion fields">
				<%# GetMessage("SymbolLabel")%><asp:TextBox ID="SymbolText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SymbolErrors" runat="server" />
				<%# GetMessage("MeasureLabel")%><asp:TextBox ID="MeasureText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MeasureErrors" runat="server" />
		</div>	
	</div>






