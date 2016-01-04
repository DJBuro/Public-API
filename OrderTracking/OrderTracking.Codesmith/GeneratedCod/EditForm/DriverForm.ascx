<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DriverForm.ascx.cs" Inherits="OrderTracking.Data.DriverForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editDriverregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("ExternalDriverIdLabel")%><asp:TextBox ID="ExternalDriverIdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ExternalDriverIdErrors" runat="server" />
				<%# GetMessage("VehicleLabel")%><asp:TextBox ID="VehicleText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="VehicleErrors" runat="server" />
		</div>	
	</div>






