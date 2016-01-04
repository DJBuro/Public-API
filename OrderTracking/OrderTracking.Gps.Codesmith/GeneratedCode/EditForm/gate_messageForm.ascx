<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GatemessageForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GatemessageForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGatemessageregion fields">
				<%# GetMessage("TrackdataidLabel")%><asp:TextBox ID="TrackdataidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TrackdataidErrors" runat="server" />
				<%# GetMessage("ServertimestampLabel")%><asp:TextBox ID="ServertimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ServertimestampErrors" runat="server" />
				<%# GetMessage("ServertimestampmsLabel")%><asp:TextBox ID="ServertimestampmsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ServertimestampmsErrors" runat="server" />
				<%# GetMessage("DeviceidLabel")%><asp:TextBox ID="DeviceidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeviceidErrors" runat="server" />
		</div>	
	</div>






