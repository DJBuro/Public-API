<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackrecorderForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TrackrecorderForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTrackrecorderregion fields">
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("TrackinfoidLabel")%><asp:TextBox ID="TrackinfoidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TrackinfoidErrors" runat="server" />
				<%# GetMessage("RecordingLabel")%><asp:TextBox ID="RecordingText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RecordingErrors" runat="server" />
				<%# GetMessage("LasttrackdataidLabel")%><asp:TextBox ID="LasttrackdataidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LasttrackdataidErrors" runat="server" />
				<%# GetMessage("TimefilterLabel")%><asp:TextBox ID="TimefilterText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimefilterErrors" runat="server" />
				<%# GetMessage("DistancefilterLabel")%><asp:TextBox ID="DistancefilterText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DistancefilterErrors" runat="server" />
				<%# GetMessage("DirectionfilterLabel")%><asp:TextBox ID="DirectionfilterText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DirectionfilterErrors" runat="server" />
				<%# GetMessage("DirectionthresholdLabel")%><asp:TextBox ID="DirectionthresholdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DirectionthresholdErrors" runat="server" />
				<%# GetMessage("SpeedfilterLabel")%><asp:TextBox ID="SpeedfilterText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SpeedfilterErrors" runat="server" />
				<%# GetMessage("RestarttimeLabel")%><asp:TextBox ID="RestarttimeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RestarttimeErrors" runat="server" />
				<%# GetMessage("RestartdistanceLabel")%><asp:TextBox ID="RestartdistanceText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RestartdistanceErrors" runat="server" />
				<%# GetMessage("TrackcategoryidLabel")%><asp:TextBox ID="TrackcategoryidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TrackcategoryidErrors" runat="server" />
				<%# GetMessage("LastuncertaintrackdataidLabel")%><asp:TextBox ID="LastuncertaintrackdataidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LastuncertaintrackdataidErrors" runat="server" />
				<%# GetMessage("RestartintervalLabel")%><asp:TextBox ID="RestartintervalText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RestartintervalErrors" runat="server" />
				<%# GetMessage("RestartintervaloffsetLabel")%><asp:TextBox ID="RestartintervaloffsetText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RestartintervaloffsetErrors" runat="server" />
				<%# GetMessage("SmstimefilterLabel")%><asp:TextBox ID="SmstimefilterText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SmstimefilterErrors" runat="server" />
				<%# GetMessage("MotionLabel")%><asp:TextBox ID="MotionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MotionErrors" runat="server" />
		</div>	
	</div>






