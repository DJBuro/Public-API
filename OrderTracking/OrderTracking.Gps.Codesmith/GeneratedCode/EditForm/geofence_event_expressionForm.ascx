<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeofenceeventexpressionForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GeofenceeventexpressionForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGeofenceeventexpressionregion fields">
				<%# GetMessage("GeofenceactionLabel")%><asp:TextBox ID="GeofenceactionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GeofenceactionErrors" runat="server" />
		</div>	
	</div>






