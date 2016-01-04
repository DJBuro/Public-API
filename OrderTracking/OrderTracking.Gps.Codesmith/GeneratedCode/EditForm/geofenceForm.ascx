<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeofenceForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GeofenceForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGeofenceregion fields">
				<%# GetMessage("GeofencenameLabel")%><asp:TextBox ID="GeofencenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GeofencenameErrors" runat="server" />
				<%# GetMessage("GeofencedescriptionLabel")%><asp:TextBox ID="GeofencedescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GeofencedescriptionErrors" runat="server" />
				<%# GetMessage("MinlongitudeLabel")%><asp:TextBox ID="MinlongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinlongitudeErrors" runat="server" />
				<%# GetMessage("MaxlongitudeLabel")%><asp:TextBox ID="MaxlongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxlongitudeErrors" runat="server" />
				<%# GetMessage("MinlatitudeLabel")%><asp:TextBox ID="MinlatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinlatitudeErrors" runat="server" />
				<%# GetMessage("MaxlatitudeLabel")%><asp:TextBox ID="MaxlatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxlatitudeErrors" runat="server" />
				<%# GetMessage("MinaltitudeLabel")%><asp:TextBox ID="MinaltitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinaltitudeErrors" runat="server" />
				<%# GetMessage("MaxaltitudeLabel")%><asp:TextBox ID="MaxaltitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxaltitudeErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
		</div>	
	</div>






