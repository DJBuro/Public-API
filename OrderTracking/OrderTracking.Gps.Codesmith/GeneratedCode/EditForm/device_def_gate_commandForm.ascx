<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DevicedefgatecommandForm.ascx.cs" Inherits="OrderTracking.Gps.Data.DevicedefgatecommandForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editDevicedefgatecommandregion fields">
				<%# GetMessage("TransportLabel")%><asp:TextBox ID="TransportText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TransportErrors" runat="server" />
		</div>	
	</div>






