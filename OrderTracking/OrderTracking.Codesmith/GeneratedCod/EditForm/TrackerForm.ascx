<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackerForm.ascx.cs" Inherits="OrderTracking.Data.TrackerForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTrackerregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("BatteryLevelLabel")%><asp:TextBox ID="BatteryLevelText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BatteryLevelErrors" runat="server" />
				<%# GetMessage("IMEILabel")%><asp:TextBox ID="IMEIText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="IMEIErrors" runat="server" />
				<%# GetMessage("SerialNumberLabel")%><asp:TextBox ID="SerialNumberText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SerialNumberErrors" runat="server" />
				<%# GetMessage("PhoneNumberLabel")%><asp:TextBox ID="PhoneNumberText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PhoneNumberErrors" runat="server" />
				<%# GetMessage("ActiveLabel")%><asp:TextBox ID="ActiveText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ActiveErrors" runat="server" />
		</div>	
	</div>






