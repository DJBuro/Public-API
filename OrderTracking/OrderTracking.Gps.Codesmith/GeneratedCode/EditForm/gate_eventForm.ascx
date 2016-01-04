<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGateeventregion fields">
				<%# GetMessage("UseridLabel")%><asp:TextBox ID="UseridText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UseridErrors" runat="server" />
				<%# GetMessage("CauseLabel")%><asp:TextBox ID="CauseText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CauseErrors" runat="server" />
				<%# GetMessage("GateeventexpressionevaluatoridLabel")%><asp:TextBox ID="GateeventexpressionevaluatoridText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GateeventexpressionevaluatoridErrors" runat="server" />
				<%# GetMessage("StageLabel")%><asp:TextBox ID="StageText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StageErrors" runat="server" />
				<%# GetMessage("MinlongitudeLabel")%><asp:TextBox ID="MinlongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinlongitudeErrors" runat="server" />
				<%# GetMessage("MaxlongitudeLabel")%><asp:TextBox ID="MaxlongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxlongitudeErrors" runat="server" />
				<%# GetMessage("MinlatitudeLabel")%><asp:TextBox ID="MinlatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinlatitudeErrors" runat="server" />
				<%# GetMessage("MaxlatitudeLabel")%><asp:TextBox ID="MaxlatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxlatitudeErrors" runat="server" />
				<%# GetMessage("MinaltitudeLabel")%><asp:TextBox ID="MinaltitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinaltitudeErrors" runat="server" />
				<%# GetMessage("MaxaltitudeLabel")%><asp:TextBox ID="MaxaltitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxaltitudeErrors" runat="server" />
				<%# GetMessage("MintimestampLabel")%><asp:TextBox ID="MintimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MintimestampErrors" runat="server" />
				<%# GetMessage("MinmillisecondsLabel")%><asp:TextBox ID="MinmillisecondsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinmillisecondsErrors" runat="server" />
				<%# GetMessage("MaxtimestampLabel")%><asp:TextBox ID="MaxtimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxtimestampErrors" runat="server" />
				<%# GetMessage("MaxmillisecondsLabel")%><asp:TextBox ID="MaxmillisecondsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxmillisecondsErrors" runat="server" />
				<%# GetMessage("ScheduledLabel")%><asp:TextBox ID="ScheduledText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ScheduledErrors" runat="server" />
		</div>	
	</div>






