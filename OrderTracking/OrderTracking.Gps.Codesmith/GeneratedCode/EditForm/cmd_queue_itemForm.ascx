<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmdqueueitemForm.ascx.cs" Inherits="OrderTracking.Gps.Data.CmdqueueitemForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editCmdqueueitemregion fields">
				<%# GetMessage("GatecommandidLabel")%><asp:TextBox ID="GatecommandidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GatecommandidErrors" runat="server" />
				<%# GetMessage("GatecommandnameLabel")%><asp:TextBox ID="GatecommandnameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GatecommandnameErrors" runat="server" />
				<%# GetMessage("StepcurrentLabel")%><asp:TextBox ID="StepcurrentText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StepcurrentErrors" runat="server" />
				<%# GetMessage("StepmaxLabel")%><asp:TextBox ID="StepmaxText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StepmaxErrors" runat="server" />
				<%# GetMessage("StepdescLabel")%><asp:TextBox ID="StepdescText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StepdescErrors" runat="server" />
				<%# GetMessage("ErrordescLabel")%><asp:TextBox ID="ErrordescText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ErrordescErrors" runat="server" />
				<%# GetMessage("CustomstateLabel")%><asp:TextBox ID="CustomstateText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CustomstateErrors" runat="server" />
				<%# GetMessage("DeliverystatusLabel")%><asp:TextBox ID="DeliverystatusText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeliverystatusErrors" runat="server" />
				<%# GetMessage("OutgoingLabel")%><asp:TextBox ID="OutgoingText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="OutgoingErrors" runat="server" />
				<%# GetMessage("TimestampclientLabel")%><asp:TextBox ID="TimestampclientText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampclientErrors" runat="server" />
				<%# GetMessage("TimestampqueuedLabel")%><asp:TextBox ID="TimestampqueuedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampqueuedErrors" runat="server" />
				<%# GetMessage("TimestampdeliveredLabel")%><asp:TextBox ID="TimestampdeliveredText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampdeliveredErrors" runat="server" />
				<%# GetMessage("TimestamplasttryLabel")%><asp:TextBox ID="TimestamplasttryText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestamplasttryErrors" runat="server" />
				<%# GetMessage("RetrycountLabel")%><asp:TextBox ID="RetrycountText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RetrycountErrors" runat="server" />
		</div>	
	</div>






