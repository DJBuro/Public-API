<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeocoderproviderForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GeocoderproviderForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGeocoderproviderregion fields">
				<%# GetMessage("TypeidLabel")%><asp:TextBox ID="TypeidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TypeidErrors" runat="server" />
				<%# GetMessage("PriorityLabel")%><asp:TextBox ID="PriorityText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PriorityErrors" runat="server" />
		</div>	
	</div>






