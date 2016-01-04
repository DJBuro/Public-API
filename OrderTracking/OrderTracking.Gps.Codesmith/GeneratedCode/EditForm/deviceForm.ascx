<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeviceForm.ascx.cs" Inherits="OrderTracking.Gps.Data.DeviceForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editDeviceregion fields">
				<%# GetMessage("DevicenameLabel")%><asp:TextBox ID="DevicenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DevicenameErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
				<%# GetMessage("HidepositionLabel")%><asp:TextBox ID="HidepositionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="HidepositionErrors" runat="server" />
				<%# GetMessage("ProximityLabel")%><asp:TextBox ID="ProximityText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProximityErrors" runat="server" />
				<%# GetMessage("IMEILabel")%><asp:TextBox ID="IMEIText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="IMEIErrors" runat="server" />
				<%# GetMessage("PhonenumberLabel")%><asp:TextBox ID="PhonenumberText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PhonenumberErrors" runat="server" />
				<%# GetMessage("LastipLabel")%><asp:TextBox ID="LastipText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LastipErrors" runat="server" />
				<%# GetMessage("LastportLabel")%><asp:TextBox ID="LastportText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LastportErrors" runat="server" />
				<%# GetMessage("StaticipLabel")%><asp:TextBox ID="StaticipText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StaticipErrors" runat="server" />
				<%# GetMessage("StaticportLabel")%><asp:TextBox ID="StaticportText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StaticportErrors" runat="server" />
				<%# GetMessage("LongitudeLabel")%><asp:TextBox ID="LongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LongitudeErrors" runat="server" />
				<%# GetMessage("LatitudeLabel")%><asp:TextBox ID="LatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LatitudeErrors" runat="server" />
				<%# GetMessage("GroundspeedLabel")%><asp:TextBox ID="GroundspeedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GroundspeedErrors" runat="server" />
				<%# GetMessage("AltitudeLabel")%><asp:TextBox ID="AltitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AltitudeErrors" runat="server" />
				<%# GetMessage("HeadingLabel")%><asp:TextBox ID="HeadingText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="HeadingErrors" runat="server" />
				<%# GetMessage("TimestampLabel")%><asp:TextBox ID="TimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampErrors" runat="server" />
				<%# GetMessage("MillisecondsLabel")%><asp:TextBox ID="MillisecondsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MillisecondsErrors" runat="server" />
				<%# GetMessage("ProtocolidLabel")%><asp:TextBox ID="ProtocolidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProtocolidErrors" runat="server" />
				<%# GetMessage("ProtocolversionidLabel")%><asp:TextBox ID="ProtocolversionidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProtocolversionidErrors" runat="server" />
				<%# GetMessage("ValidLabel")%><asp:TextBox ID="ValidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValidErrors" runat="server" />
				<%# GetMessage("ApnLabel")%><asp:TextBox ID="ApnText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ApnErrors" runat="server" />
				<%# GetMessage("GprsusernameLabel")%><asp:TextBox ID="GprsusernameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GprsusernameErrors" runat="server" />
				<%# GetMessage("GprspasswordLabel")%><asp:TextBox ID="GprspasswordText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GprspasswordErrors" runat="server" />
				<%# GetMessage("DevdefidLabel")%><asp:TextBox ID="DevdefidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DevdefidErrors" runat="server" />
				<%# GetMessage("OutgoingtransportLabel")%><asp:TextBox ID="OutgoingtransportText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="OutgoingtransportErrors" runat="server" />
				<%# GetMessage("EmailLabel")%><asp:TextBox ID="EmailText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EmailErrors" runat="server" />
		</div>	
	</div>






