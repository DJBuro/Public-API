<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackdatamodForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TrackdatamodForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTrackdatamodregion fields">
				<%# GetMessage("LongitudeLabel")%><asp:TextBox ID="LongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LongitudeErrors" runat="server" />
				<%# GetMessage("LatitudeLabel")%><asp:TextBox ID="LatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LatitudeErrors" runat="server" />
				<%# GetMessage("AltitudeLabel")%><asp:TextBox ID="AltitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AltitudeErrors" runat="server" />
				<%# GetMessage("HeadingLabel")%><asp:TextBox ID="HeadingText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="HeadingErrors" runat="server" />
				<%# GetMessage("GroundspeedLabel")%><asp:TextBox ID="GroundspeedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GroundspeedErrors" runat="server" />
				<%# GetMessage("TimestampLabel")%><asp:TextBox ID="TimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampErrors" runat="server" />
				<%# GetMessage("MillisecondsLabel")%><asp:TextBox ID="MillisecondsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MillisecondsErrors" runat="server" />
				<%# GetMessage("ValidLabel")%><asp:TextBox ID="ValidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ValidErrors" runat="server" />
				<%# GetMessage("DeletedLabel")%><asp:TextBox ID="DeletedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeletedErrors" runat="server" />
		</div>	
	</div>






