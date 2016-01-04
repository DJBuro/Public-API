<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProvidermessageForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ProvidermessageForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editProvidermessageregion fields">
				<%# GetMessage("ClientdeviceidLabel")%><asp:TextBox ID="ClientdeviceidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ClientdeviceidErrors" runat="server" />
				<%# GetMessage("ClientaddressLabel")%><asp:TextBox ID="ClientaddressText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ClientaddressErrors" runat="server" />
				<%# GetMessage("MessageLabel")%><asp:TextBox ID="MessageText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MessageErrors" runat="server" />
				<%# GetMessage("DeliverystatusLabel")%><asp:TextBox ID="DeliverystatusText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeliverystatusErrors" runat="server" />
				<%# GetMessage("OutgoingLabel")%><asp:TextBox ID="OutgoingText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="OutgoingErrors" runat="server" />
				<%# GetMessage("TimestampclientLabel")%><asp:TextBox ID="TimestampclientText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampclientErrors" runat="server" />
				<%# GetMessage("TimestampqueuedLabel")%><asp:TextBox ID="TimestampqueuedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampqueuedErrors" runat="server" />
				<%# GetMessage("TimestampdeliveredLabel")%><asp:TextBox ID="TimestampdeliveredText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampdeliveredErrors" runat="server" />
				<%# GetMessage("TimestamplasttryLabel")%><asp:TextBox ID="TimestamplasttryText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestamplasttryErrors" runat="server" />
				<%# GetMessage("RetrycountLabel")%><asp:TextBox ID="RetrycountText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RetrycountErrors" runat="server" />
				<%# GetMessage("TransportLabel")%><asp:TextBox ID="TransportText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TransportErrors" runat="server" />
		</div>	
	</div>






