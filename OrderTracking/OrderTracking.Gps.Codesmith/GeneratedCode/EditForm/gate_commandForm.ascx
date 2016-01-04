<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GatecommandForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GatecommandForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGatecommandregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("EnabledLabel")%><asp:TextBox ID="EnabledText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EnabledErrors" runat="server" />
				<%# GetMessage("NamespaceLabel")%><asp:TextBox ID="NamespaceText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NamespaceErrors" runat="server" />
				<%# GetMessage("OutgoingLabel")%><asp:TextBox ID="OutgoingText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="OutgoingErrors" runat="server" />
				<%# GetMessage("SpecialLabel")%><asp:TextBox ID="SpecialText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SpecialErrors" runat="server" />
		</div>	
	</div>






