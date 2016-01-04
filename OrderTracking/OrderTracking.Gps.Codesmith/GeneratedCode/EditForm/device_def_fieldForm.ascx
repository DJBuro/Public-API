<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DevicedeffieldForm.ascx.cs" Inherits="OrderTracking.Gps.Data.DevicedeffieldForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editDevicedeffieldregion fields">
				<%# GetMessage("SavechangesonlyLabel")%><asp:TextBox ID="SavechangesonlyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SavechangesonlyErrors" runat="server" />
		</div>	
	</div>






