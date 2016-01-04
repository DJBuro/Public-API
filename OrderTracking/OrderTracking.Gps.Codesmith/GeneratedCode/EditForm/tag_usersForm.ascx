<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaguserForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TaguserForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTaguserregion fields">
				<%# GetMessage("ServertimestampLabel")%><asp:TextBox ID="ServertimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ServertimestampErrors" runat="server" />
				<%# GetMessage("ServertimestampmsLabel")%><asp:TextBox ID="ServertimestampmsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ServertimestampmsErrors" runat="server" />
		</div>	
	</div>






