<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MsgnamespaceForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MsgnamespaceForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editMsgnamespaceregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("ProtocolidLabel")%><asp:TextBox ID="ProtocolidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProtocolidErrors" runat="server" />
		</div>	
	</div>






