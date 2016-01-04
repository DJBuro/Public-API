<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderForm.ascx.cs" Inherits="OrderTracking.Data.OrderForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editOrderregion fields">
				<%# GetMessage("ExternalOrderIdLabel")%><asp:TextBox ID="ExternalOrderIdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ExternalOrderIdErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("TicketNumberLabel")%><asp:TextBox ID="TicketNumberText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TicketNumberErrors" runat="server" />
				<%# GetMessage("ExtraInformationLabel")%><asp:TextBox ID="ExtraInformationText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ExtraInformationErrors" runat="server" />
				<%# GetMessage("ProximityDeliveredLabel")%><asp:TextBox ID="ProximityDeliveredText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProximityDeliveredErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
		</div>	
	</div>






