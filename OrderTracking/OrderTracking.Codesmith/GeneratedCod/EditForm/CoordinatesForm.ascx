<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoordinateForm.ascx.cs" Inherits="OrderTracking.Data.CoordinateForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editCoordinateregion fields">
				<%# GetMessage("LongitudeLabel")%><asp:TextBox ID="LongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LongitudeErrors" runat="server" />
				<%# GetMessage("LatitudeLabel")%><asp:TextBox ID="LatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LatitudeErrors" runat="server" />
		</div>	
	</div>






