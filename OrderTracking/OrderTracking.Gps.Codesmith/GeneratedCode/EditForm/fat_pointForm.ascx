<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FatpointForm.ascx.cs" Inherits="OrderTracking.Gps.Data.FatpointForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editFatpointregion fields">
				<%# GetMessage("StarttrackdataidLabel")%><asp:TextBox ID="StarttrackdataidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StarttrackdataidErrors" runat="server" />
				<%# GetMessage("EndtrackdataidLabel")%><asp:TextBox ID="EndtrackdataidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EndtrackdataidErrors" runat="server" />
				<%# GetMessage("LongitudeLabel")%><asp:TextBox ID="LongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LongitudeErrors" runat="server" />
				<%# GetMessage("LatitudeLabel")%><asp:TextBox ID="LatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LatitudeErrors" runat="server" />
				<%# GetMessage("AltitudeLabel")%><asp:TextBox ID="AltitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AltitudeErrors" runat="server" />
				<%# GetMessage("StarttimeLabel")%><asp:TextBox ID="StarttimeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StarttimeErrors" runat="server" />
				<%# GetMessage("StarttimemsLabel")%><asp:TextBox ID="StarttimemsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StarttimemsErrors" runat="server" />
				<%# GetMessage("EndtimeLabel")%><asp:TextBox ID="EndtimeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EndtimeErrors" runat="server" />
				<%# GetMessage("EndtimemsLabel")%><asp:TextBox ID="EndtimemsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EndtimemsErrors" runat="server" />
				<%# GetMessage("ErrorradiusLabel")%><asp:TextBox ID="ErrorradiusText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ErrorradiusErrors" runat="server" />
				<%# GetMessage("BuilddotsLabel")%><asp:TextBox ID="BuilddotsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BuilddotsErrors" runat="server" />
		</div>	
	</div>






