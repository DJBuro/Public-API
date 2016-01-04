<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateviewForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateviewForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGateviewregion fields">
				<%# GetMessage("ViewnameLabel")%><asp:TextBox ID="ViewnameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ViewnameErrors" runat="server" />
				<%# GetMessage("ViewdescriptionLabel")%><asp:TextBox ID="ViewdescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ViewdescriptionErrors" runat="server" />
				<%# GetMessage("StatusfilterLabel")%><asp:TextBox ID="StatusfilterText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StatusfilterErrors" runat="server" />
				<%# GetMessage("MatchalltagsLabel")%><asp:TextBox ID="MatchalltagsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MatchalltagsErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
		</div>	
	</div>






