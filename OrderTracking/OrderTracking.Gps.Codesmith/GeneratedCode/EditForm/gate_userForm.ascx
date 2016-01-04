<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateuserForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateuserForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGateuserregion fields">
				<%# GetMessage("LongitudeLabel")%><asp:TextBox ID="LongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LongitudeErrors" runat="server" />
				<%# GetMessage("LatitudeLabel")%><asp:TextBox ID="LatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LatitudeErrors" runat="server" />
				<%# GetMessage("GroundspeedLabel")%><asp:TextBox ID="GroundspeedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GroundspeedErrors" runat="server" />
				<%# GetMessage("AltitudeLabel")%><asp:TextBox ID="AltitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AltitudeErrors" runat="server" />
				<%# GetMessage("HeadingLabel")%><asp:TextBox ID="HeadingText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="HeadingErrors" runat="server" />
				<%# GetMessage("TimestampLabel")%><asp:TextBox ID="TimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampErrors" runat="server" />
				<%# GetMessage("ServertimestampLabel")%><asp:TextBox ID="ServertimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ServertimestampErrors" runat="server" />
				<%# GetMessage("DeviceactivityLabel")%><asp:TextBox ID="DeviceactivityText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeviceactivityErrors" runat="server" />
				<%# GetMessage("DelayLabel")%><asp:TextBox ID="DelayText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DelayErrors" runat="server" />
				<%# GetMessage("LasttransportLabel")%><asp:TextBox ID="LasttransportText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LasttransportErrors" runat="server" />
		</div>	
	</div>






