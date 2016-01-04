<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapmetadataForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MapmetadataForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editMapmetadataregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("ValueLabel")%><asp:TextBox ID="ValueText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValueErrors" runat="server" />
		</div>	
	</div>






